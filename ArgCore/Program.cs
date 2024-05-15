using Arg.DataAccess;
using ArgCore.Data;
using ArgCore.Helpers;
using ArgCore.Models;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static ArgCore.Helpers.IdentityConfig;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
});

builder.Configuration.AddJsonFile("appsettings.json").AddJsonFile("myconfig.json");

builder.Services.AddHttpContextAccessor();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DriveService>();
builder.Services.AddScoped<DapperUserStore>();
builder.Services.AddScoped<DapperContext>();
builder.Services.AddDistributedMemoryCache();

// Identity setup
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddUserStore<DapperUserStore>()
    .AddRoleStore<DapperUserStore>()
    .AddDefaultTokenProviders();

// Session service
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cookie service
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// Add IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<ApplicationSignInManager>();
builder.Services.AddScoped<ApplicationUserManager>();

// Add on 20-3-2024
//builder.Services.AddScoped<ISmsService , SmsService>();
//builder.Services.AddScoped<IEmailService , EmailServices>();

builder.Services.AddScoped<IUserStore<ApplicationUser>>(provider =>
    new DapperUserStore(provider.GetRequiredService<DapperContext>()));
builder.Services.AddScoped<IUserRoleStore<ApplicationUser>>(provider =>
    new DapperUserStore(provider.GetRequiredService<DapperContext>()));
builder.Services.AddScoped<IUserPasswordStore<ApplicationUser>>(provider =>
    new DapperUserStore(provider.GetRequiredService<DapperContext>()));
builder.Services.AddScoped<IUserEmailStore<ApplicationUser>>(provider =>
    new DapperUserStore(provider.GetRequiredService<DapperContext>()));

builder.Services.AddScoped<ApplicationUserManager>(provider =>
{
    var userStore = new DapperUserStore(provider.GetRequiredService<DapperContext>());

    var identityOptions = provider.GetRequiredService<IOptions<IdentityOptions>>();
    var passwordHasher = provider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

    // Retrieve the default UserValidators provided by Identity
    var userValidators = provider.GetServices<IUserValidator<ApplicationUser>>();

    return new ApplicationUserManager(
        userStore,
        identityOptions,
        passwordHasher,
        userValidators,
        Enumerable.Empty<IPasswordValidator<ApplicationUser>>(),
        provider.GetRequiredService<ILookupNormalizer>(),
        provider.GetRequiredService<IdentityErrorDescriber>(),
        provider,
        provider.GetRequiredService<ILogger<UserManager<ApplicationUser>>>()
    );
});

builder.Services.AddScoped<ApplicationSignInManager>();


var app = builder.Build();

Arg.DataAccess.Common.SetHttpContextAccessor(app.Configuration, app.Services.GetRequiredService<IHttpContextAccessor>());
ActiveClient.SetHttpContextAccessor(app.Services.GetRequiredService<IHttpContextAccessor>());
ArgCore.Helpers.Common.SetHttpContextAccessor(app.Configuration, app.Services.GetRequiredService<IHttpContextAccessor>(), app.Services.GetRequiredService<IWebHostEnvironment>());


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TemplateCats}/{action=Index}/{id?}");

app.Run();
