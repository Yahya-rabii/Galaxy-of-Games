using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using mvc_gog.Data;
using mvc_gog.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<mvc_gogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("mvc_gogContext") ?? throw new InvalidOperationException("Connection string 'mvc_gogContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
   
options.LoginPath = "/Users/Login";
options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
options.SlidingExpiration = true;
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("isadmin", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("isuser", policy => policy.RequireClaim(ClaimTypes.Role, "user"));
});


builder.Services.AddSession();


var app = builder.Build();

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

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && context.Request.Path.Value != "/Users/Login")
    {
        context.Response.Redirect("/Users/Login");
    }
    else if (context.User.Identity.IsAuthenticated && context.User.IsInRole("admin") && context.Request.Path.Value == "/Users/Login")
    {
        context.Response.Redirect("/");
    }  
    else if (context.User.Identity.IsAuthenticated && context.User.IsInRole("user") && context.Request.Path.Value == "/Users/Login")
    {
        context.Response.Redirect("/Produits/List");
    }
    else
    {
        await next();
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Produits}/{action=List}/{id?}");
});


app.Run();

