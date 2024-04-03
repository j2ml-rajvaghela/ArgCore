
using ArgCore.Data;
using ArgCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ArgCore.Helpers
{
 
    public class IdentityConfig
    {
        public class ApplicationUserManager : UserManager<ApplicationUser>
        {

            public ApplicationUserManager(
            DapperUserStore userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

            public static ApplicationUserManager Create(IServiceProvider serviceProvider)
            {
                var userStore = new DapperUserStore(serviceProvider.GetRequiredService<DapperContext>());
                var optionsAccessor = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>();
                var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();
                var userValidators = serviceProvider.GetRequiredService<IEnumerable<IUserValidator<ApplicationUser>>>();
                var passwordValidators = serviceProvider.GetRequiredService<IEnumerable<IPasswordValidator<ApplicationUser>>>();
                var keyNormalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
                var errors = serviceProvider.GetRequiredService<IdentityErrorDescriber>();
                var services = serviceProvider.GetRequiredService<IServiceProvider>();
                var logger = serviceProvider.GetRequiredService<ILogger<UserManager<ApplicationUser>>>();

                var userManager = new ApplicationUserManager(
                    userStore,
                    optionsAccessor,
                    passwordHasher,
                    userValidators,
                    passwordValidators,
                    keyNormalizer,
                    errors,
                    services,
                    logger
                );
                return userManager;
            }
        }

        public class ApplicationSignInManager : SignInManager<ApplicationUser>
        {
            public ApplicationSignInManager(
                ApplicationUserManager userManager,
                IHttpContextAccessor contextAccessor,
                IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                IOptions<IdentityOptions> optionsAccessor,
                ILogger<SignInManager<ApplicationUser>> logger,
                IAuthenticationSchemeProvider schemeProvider,
                IUserConfirmation<ApplicationUser> confirmation)
                : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation) { }

            public static ApplicationSignInManager Create(IServiceProvider serviceProvider)
            {
                var userManager = serviceProvider.GetRequiredService<ApplicationUserManager>();
                var contextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var claimsFactory = serviceProvider.GetRequiredService<IUserClaimsPrincipalFactory<ApplicationUser>>();
                var optionsAccessor = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>();
                var logger = serviceProvider.GetRequiredService<ILogger<SignInManager<ApplicationUser>>>();
                var schemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
                var confirmation = serviceProvider.GetRequiredService<IUserConfirmation<ApplicationUser>>();

                return new ApplicationSignInManager(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation);
            }
        }
    }
}
