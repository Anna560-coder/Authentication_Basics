using _220008504_AuthBasics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
    .AddCookie(Settings.AuthCookieName,
    options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Forbidden";
        options.Cookie.Name = Settings.AuthCookieName;

        
        options.Cookie.HttpOnly = true; 
        options.Cookie.SameSite = SameSiteMode.Strict; 
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 

        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

    builder.Services.AddAuthorization(
        options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireClaim("admin", "true"));
        });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});


var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();


app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "defaut",
    pattern: "{controller=Home}/{action=Index}");


app.Run();
