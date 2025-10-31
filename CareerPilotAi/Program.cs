using CareerPilotAi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using CareerPilotAi.Prompts;
using CareerPilotAi.Application.Commands;
using CareerPilotAi.Application;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .RegisterPrompts()
    .AddHttpContextAccessor()
    .AddInfrastructure(builder.Configuration)
    .RegisterCommands()
    .AddApplication();

services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Skip HTTPS redirection for E2E tests (they use HTTP only.)
if (app.Environment.EnvironmentName != "e2e")
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

// Add Identity middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
