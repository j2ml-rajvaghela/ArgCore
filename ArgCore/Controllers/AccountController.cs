using Arg.DAL;
using Arg.DataModels;
using ArgCore.Attributes;
using ArgCore.Helpers;
using ArgCore.Models;
using Google.Authenticator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security;
using SmartFormat;
using System.Security.Claims;
using System.Text;
using static ArgCore.Helpers.IdentityConfig;

namespace ArgCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private readonly DapperUserStore _dapperUserStore;
        private readonly IEmailSender _emailSender;
        private bool isSetupTFA = false;


        public AccountController(ApplicationUserManager userManager, DapperUserStore dapperUserStore, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _dapperUserStore = dapperUserStore;
            _signInManager = signInManager;

        }

        [AllowAnonymous]
        //[IpAuth]
        public IActionResult LogIn(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                HttpContext.Session.SetString("IsSessionActive", "1");
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.ToString());
                return null;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[IpAuth]
        public async Task<IActionResult> LogIn(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
                var user = await _dapperUserStore.FindByEmailAsync(model.Email, CancellationToken.None);
                LogInAttempt ATT = new LogInAttempt();
                ATT.IPAddress = Arg.DataAccess.Common.GetIPAddress();

                if (user == null)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.LoginFailed, 0, "Email: " + model.Email);
                    ModelState.AddModelError("", "Invalid login attempt.");
                    using (users db = new users())
                    {
                        ATT.ErrorMessage = "Invalid login attempt.";
                        db.insert_loginattempt(ATT);
                    }
                    return View(model);
                }

                if (user.AccessFailedCount >= Common.FailedLoginAttempt)
                {
                    ModelState.AddModelError("", "User locked due to too many failed login attempts. Please contact Administrator.");
                    return View(model);
                }

                if (user.LoginValidUntil.Date == new DateTime(1900, 1, 1))
                {
                    user.Status = true;
                    user.LoginValidUntil = DateTime.Now.AddMonths(3);
                    UpdateUser(user);
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Administrator"))
                {
                    if (user.LoginValidUntil <= DateTime.Now || user.Status == false)
                    {
                        user.Status = false;
                        UpdateUser(user);
                        ModelState.AddModelError("", "User is disabled. Contact Administrator.");
                        return View(model);
                    }
                }

                ATT.UserName = model.Email;
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                var signInResult = new Microsoft.AspNetCore.Identity.SignInResult();

                switch (result)
                {
                    case var _ when signInResult.Succeeded:

                        if (user.AccessFailedCount != 0)
                            ResetFailedLoginAttempt(user);

                        using (users db = new users())
                        {
                            ATT.ErrorMessage = "";
                            ATT.Autheticated = true;
                            db.insert_loginattempt(ATT);
                        }

                        TempData["Email"] = model.Email;
                        TempData["UserUniqueKey"] = user.SecurityStamp.ToString() + Common.GoogleAuthKey;

                        if (Common.EnableTFA == 1)
                        {
                            TempData["TwoFactorEnabled"] = user.TwoFactorEnabled;
                            if (user.TwoFactorEnabled)
                            {
                                return RedirectToAction("TwoFactorAuth");
                            }
                            else
                            {
                                return RedirectToAction("SetupTwoFactorAuth");
                            }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Admin");
                        }

                    case var _ when signInResult.IsLockedOut:
                        return View("Lockout");

                    case var _ when signInResult.RequiresTwoFactor:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                    case var _ when result == Microsoft.AspNetCore.Identity.SignInResult.Failed:
                    //case var _ when signInResult.IsNotAllowed:
                    default:
                        await SaveFailedLoginAttempt(user);
                        ModelState.AddModelError("", "Invalid login attempt. User will be locked after " + Common.FailedLoginAttempt + " failed attempt.");
                        Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.InvalidLoginAttempt, 0, "Email: " + model.Email);
                        using (users db = new users())
                        {
                            ATT.ErrorMessage = "Invalid login attempt.";
                            db.insert_loginattempt(ATT);
                        }

                        return View(model);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        public IActionResult TwoFactorAuth()
        {
            try
            {
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.LoggedIn, Common.GetActiveClientId(), "Verify TFA code");
                var model = new TFAViewModel
                {
                    Email = (string)TempData["Email"],
                    UserUniqueKey = (string)TempData["UserUniqueKey"],
                    TwoFactorEnabled = (bool)TempData["TwoFactorEnabled"]
                };
                return View(model);

            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult TwoFactorAuthenticate(TFAViewModel model)
        {
            var user = _userManager.FindByEmailAsync(model.Email).Result;

            if (!string.IsNullOrEmpty(model.TFACode))
            {
                TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                bool isValid = TwoFacAuth.ValidateTwoFactorPIN(model.UserUniqueKey, model.TFACode, false);

                if (isValid)
                {
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.LoggedIn, Common.GetActiveClientId(), "TFA verified.");

                    if (user.LastPasswordChangeDate.Date.AddDays(90) < DateTime.Now.Date)
                    {
                        return RedirectToAction("ChangePassword", "Manage");
                    }
                    else
                    {

                        return RedirectToAction("Index", "Admin");
                    }

                }
                else
                {
                    var errorMessage = "Two Factor Authentication PIN is expired or wrong.";

                    ModelState.AddModelError("", errorMessage);
                    Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.InvalidTFAAttempt, 0, "Email: " + model.Email);

                    using (users db = new users())
                    {
                        LogInAttempt ATT = new LogInAttempt();
                        ATT.Autheticated = false;
                        ATT.IPAddress = Arg.DataAccess.Common.GetIPAddress();
                        ATT.UserName = model.Email;
                        ATT.ErrorMessage = errorMessage;
                        db.insert_loginattempt(ATT);
                    }
                }

                return View("TwoFactorAuth", model);
            }
            return RedirectToAction("Login");
        }

        public IActionResult SetupTwoFactorAuth()
        {
            try
            {
                var viewModel = new TFAViewModel
                {
                    Email = (string)TempData["Email"],
                    UserUniqueKey = (string)TempData["UserUniqueKey"],
                    TwoFactorEnabled = (bool)TempData["TwoFactorEnabled"]
                };

                //Two Factor Authentication Setup
                TwoFactorAuthenticator twoFactorAuthenticator = new TwoFactorAuthenticator();
                var setupInfo = twoFactorAuthenticator.GenerateSetupCode(Common.TFAIssuer, viewModel.Email, Encoding.UTF8.GetBytes(viewModel.UserUniqueKey), 300);

                viewModel.BarCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                viewModel.SetupCode = setupInfo.ManualEntryKey;

                return View(viewModel);
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult SetupTwoFactorAuth(TFAViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Email))
            {
                return RedirectToAction("Login");
            }

            var user = _userManager.FindByEmailAsync(viewModel.Email).Result;

            TempData["Email"] = viewModel.Email;
            TempData["UserUniqueKey"] = viewModel.UserUniqueKey;

            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            bool isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(viewModel.UserUniqueKey, viewModel.TFACode.ToString(), false);

            if (isValid)
            {
                //update TFA
                user.TwoFactorEnabled = true;
                UpdateUser(user);

                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.SetupTFA, Common.GetActiveClientId(), "Setup two Factor Authentication");

                TempData["TwoFactorEnabled"] = user.TwoFactorEnabled;
                return RedirectToAction("TwoFactorAuth");
            }

            ViewBag.Message = "Two Factor Authentication PIN is expired or wrong.";
            return View(viewModel);
        }


        [Authorize(Policy = "AllowAnonymousPolicy")]
        public async Task<IActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            try
            {   // Require that the user has already logged in via username/password or external login
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    return View("Error");
                }

                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                // The following code protects for brute force attacks against the two factor codes.
                // If a user enters incorrect codes for a specified amount of time then the user account
                // will be locked out for a specified amount of time.
                // You can configure the account lockout settings in IdentityConfig
                var result = await _signInManager.TwoFactorSignInAsync(viewModel.Provider, viewModel.Code, isPersistent: viewModel.RememberMe, rememberClient: viewModel.RememberBrowser);
                if (result.Succeeded)
                {
                    return RedirectToLocal(viewModel.ReturnUrl);
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid code.");
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult RegistrationThankYou()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            try
            {
                // custom
                new Common.CommonObjects().TopHeading = "Register";
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        //UserName = model.UserName,
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Status = true,
                        LoginValidUntil = DateTime.Now.AddMonths(3)
                    };

                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme); // Request.Url.Scheme
                        //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        try
                        {
                            using (users db = new users())
                            {
                                AppUser USR = new AppUser();
                                USR.AspnetUserId = user.Id;
                                USR.ClientCode = viewModel.clientcode;
                                USR.FirstName = viewModel.FirstName;
                                USR.LastName = viewModel.LastName;
                                USR.Title = viewModel.title;
                                db.insertuser(USR);
                            }

                            using (EmailService EMS = new EmailService())
                            {
                                Email EM = new Email();
                                EM.Subject = "ARG Ocean: Please Confirm your account";
                                EM.SingleRecipient = user.Email;
                                EM.Body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                                EMS.sendMessage(EM);
                            }

                        }
                        catch (Exception ex)
                        {
                            Common.Log.Error(ex);
                        }
                        return RedirectToAction("RegistrationThankYou", "Account");
                    }
                    AddErrors(result);
                }
                // If we got this far, something failed, redisplay form
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ConfirmEmailAsync(user, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(viewModel.Email);
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    if (user == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);
                    var adminEmailContent = GetAdminEmailContent(user, callbackUrl, "Please reset your password by clicking this link");
                    await _emailSender.SendEmailAsync(user.Id, "Confirm your account", adminEmailContent);
                    //try
                    //{
                    //    using (arg.utilities.emailservice EMS = new arg.utilities.emailservice())
                    //    {
                    //        arg.models.email EM = new arg.models.email();
                    //        EM.subject = "ARG Password Reset";
                    //        EM.singlerecipient = user.Email;
                    //        EM.body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    //        EMS.sendMessage(EM);
                    //    }
                    //}
                    //catch (Exception e)
                    //{
                    //    throw;
                    //}
                    // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // If we got this far, something failed, redisplay form
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        public string GetAdminEmailContent(ApplicationUser user, string link, string text)
        {
            try
            {
                //var content = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/AdminEmail.txt"));
                var content = Common.Templates.GetTemplate(0, "Admin Email");
                var data = new
                {
                    Name = user.FirstName + " " + user.LastName,
                    UserName = user.UserName,
                    PhoneNo = user.PhoneNumber,
                    Email = user.Email,
                    Link = link,
                    Text = text
                };
                return Smart.Format(content.Content, data);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var user = await _userManager.FindByNameAsync(viewModel.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                var result = await _userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);
                if (result.Succeeded)
                {
                    var userData = Common.AspNetUsers.GetAspNetUser(user.Id);
                    userData.LastPasswordChangeDate = DateTime.Now;
                    Common.AspNetUsers.SaveAspNetUser(userData);

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                AddErrors(result);
                return View();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            try
            {  // Request a redirect to the external login provider
                return new ChallengeResult(provider, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl })
                });
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            try
            {
                var user = await _signInManager.UserManager.GetUserAsync(User);

                if (user == null)
                {
                    return View("Error");
                }

                var userId = await _userManager.GetUserIdAsync(user);
                if (userId == null)
                {
                    return View("Error");
                }

                var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
                var factorOptions = userFactors.Select(purpose => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Generate the token and send it
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    return View("Error");
                }

                var token = await _userManager.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider);
                if (string.IsNullOrEmpty(token))
                {
                    return View("Error");
                }

                return RedirectToAction("VerifyCode", new { Provider = viewModel.SelectedProvider, ReturnUrl = viewModel.ReturnUrl, RememberMe = viewModel.RememberMe });
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            try
            {
                var loginInfo = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                if (loginInfo == null)
                {
                    return RedirectToAction("Login");
                }

                var emailClaim = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                var providerClaim = loginInfo.Principal.FindFirstValue(ClaimTypes.AuthenticationMethod);

                // Sign in the user with this external login provider if the user already has a login
                var result = await _signInManager.ExternalLoginSignInAsync(emailClaim, providerClaim, isPersistent: false, bypassTwoFactor: true);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                }
                else
                {
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = providerClaim;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = emailClaim });
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel viewModel, string returnUrl)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Manage");
                }

                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                    if (info == null)
                    {
                        return View("ExternalLoginFailure");
                    }
                    if (!(info is Microsoft.AspNetCore.Authentication.AuthenticateResult authenticateResult))
                    {
                        // Handle the case where info is not of the expected type
                        return View("ExternalLoginFailure");
                    }
                    var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(user, new Microsoft.AspNetCore.Identity.UserLoginInfo(authenticateResult.Principal.Identity.AuthenticationType, authenticateResult.Principal.Identity.Name, authenticateResult.Principal.Identity.AuthenticationType));
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        public IActionResult LogOff()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Common.ActivityStats.SaveActivityStats(Arg.DataAccess.ActivityStatsImpl.EnumActions.LoggedOff, Common.GetActiveClientId());
                string isSessionActive = HttpContext.Session.GetString("IsSessionActive");

                if (isSessionActive == "1")
                {
                    HttpContext.Session.Clear();
                }

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }


        [AllowAnonymous]
        public IActionResult ExternalLoginFailure()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult AccessDenied(string companyId)
        {
            ViewBag.CompanyID = companyId;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_userManager != null)
                    {
                        _userManager.Dispose();
                        _userManager = null;
                    }

                    if (_signInManager != null)
                    {
                        _signInManager.UserManager.Dispose();
                        _signInManager = null;
                    }
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }


        #region HELPERS

        private const string XsrfKey = "XsrfId";

        public IAuthenticationManager AuthenticationManager => AuthenticationManager;

        private void AddErrors(Microsoft.AspNetCore.Identity.IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //      : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public void ExecuteResult(Microsoft.AspNetCore.Mvc.ControllerContext context)
        //    {
        //        var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Items[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.ChallengeAsync(LoginProvider, properties);
        //    }
        //}

        #endregion

        public void UpdateUser(ApplicationUser user)
        {
            try
            {
                AspNetUsers data = AppUserToAspNetUser(user);
                Common.AspNetUsers.SaveAspNetUser(data);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        private async Task SaveFailedLoginAttempt(ApplicationUser user)
        {
            var data = AppUserToAspNetUser(user);
            data.AccessFailedCount = user.AccessFailedCount + 1;
            Common.AspNetUsers.SaveAspNetUser(data);

            if (data.AccessFailedCount >= Common.FailedLoginAttempt)
            {
                var adminRole = Common.AspNetRoles.GetAspNetRoleByName("Administrator");

                var searchOptions = new SearchOptions();
                searchOptions.Roles = new List<string>
                {
                    adminRole.Id
                };

                var usersList = Common.AspNetUsers.GetAspNetUsers(searchOptions);

                foreach (var item in usersList)
                {
                    var emailContent = GetEmailContent(user, item);
                    await _emailSender.SendEmailAsync(item.Id, $"ATLAS Alert:  User {user.FirstName} {user.LastName} locked due to too many failed login attempts", emailContent);
                }
            }
        }

        private string GetEmailContent(ApplicationUser user, AspNetUsers item)
        {
            try
            {
                string ip = Arg.DataAccess.Common.GetIPAddress();

                var content = Common.Templates.GetTemplate(0, "User locked");
                var data = new
                {
                    Name = item.FirstName + " " + item.LastName,
                    Email = user.Email,
                    IPAddress = ip,
                    PhoneNo = user.PhoneNumber,
                };
                return Smart.Format(content.Content, data);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return null;
            }
        }

        private void ResetFailedLoginAttempt(ApplicationUser user)
        {
            var data = AppUserToAspNetUser(user);
            data.AccessFailedCount = 0;
            Common.AspNetUsers.SaveAspNetUser(data);
        }

        [HttpPost]
        public async Task<JsonResult> ResetTFA(string userId)
        {
            try
            {
                var user = await  _userManager.FindByIdAsync(userId);
                user.TwoFactorEnabled = false;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(result.Succeeded);
                }
                return Json("MFA can't be reset!");
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                return Json(ex.ToString());
            }
        }

        private AspNetUsers AppUserToAspNetUser(ApplicationUser user)
        {
            return new AspNetUsers
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PasswordHash = user.PasswordHash,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SecurityStamp = user.SecurityStamp,
                AccessFailedCount = user.AccessFailedCount,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = Convert.ToString(user.LockoutEndDateUtc),
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Status = user.Status,
                LoginValidUntil = Convert.ToDateTime(user.LoginValidUntil),
                LastPasswordChangeDate = Convert.ToDateTime(user.LastPasswordChangeDate)
            };
        }
    }
}
