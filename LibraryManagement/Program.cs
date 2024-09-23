using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container for MVC controllers and views.
builder.Services.AddControllersWithViews();

// Configure cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    // Set the path for the login page
    options.LoginPath = new PathString("/");
    // Set the path for access denied page
    options.AccessDeniedPath = new PathString("/");
    // Set the path for logout page
    options.LogoutPath = new PathString("/");
});

var app = builder.Build();

// Serve static files (like CSS, JavaScript, images)
app.UseStaticFiles();

// Enable authentication middleware
app.UseAuthentication();

// Define the default route for the application
app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
);

// Run the application  
app.Run();
