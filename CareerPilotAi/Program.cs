using CareerPilotAi.Infrastructure;
using CareerPilotAi.Services;
using Microsoft.AspNetCore.Mvc;
using CareerPilotAi.Prompts;
using CareerPilotAi.Application.Commands;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .RegisterPrompts()
    .AddHttpContextAccessor()
    .AddInfrastructure(builder.Configuration)
    .RegisterCommands()
    .AddScoped<IUserService, UserService>();

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add Identity middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
