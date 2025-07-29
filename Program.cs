using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System;
using WeatherApp.Data;
using WeatherApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString =
    $"Server=mysql-139b5804-weather-app-fcs.k.aivencloud.com;Port=25934;Database=blazor_weather_db;Uid=avnadmin;Pwd={dbPassword};SslMode=Required;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)))
);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication((CookieAuthenticationDefaults.AuthenticationScheme))
    .AddCookie(
        options =>
        {
            options.Cookie.Name = "authCookie";
            options.LoginPath = "/login";
            options.Cookie.MaxAge = TimeSpan.FromMinutes((20));
            options.AccessDeniedPath = "/accessdenied";
        }
    );

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
