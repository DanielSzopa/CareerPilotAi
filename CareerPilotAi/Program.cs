using System.Net.Http.Headers;
using CareerPilotAi.Infrastructure;
using CareerPilotAi.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
.AddHttpContextAccessor()
.AddInfrastructure(builder.Configuration)
.AddScoped<IUserService, UserService>();


services.AddHttpClient("OpenRouter", (client) =>
{
    var baseAddress = builder.Configuration["OpenRouter:BaseAddress"] ?? throw new ArgumentNullException("BaseAddress");
    var authToken = builder.Configuration["OpenRouter:AuthToken"] ?? throw new ArgumentNullException("AuthToken");

    client.BaseAddress = new Uri(baseAddress);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
});

services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// Add Identity middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
