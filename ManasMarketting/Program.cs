using BusinessLogic.Concrete;
using BusinessLogic.Repository;
using ManasMarketting.Controllers;
using ManasMarketting.Models;
using Microsoft.AspNetCore.Authentication.Negotiate;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ILogin, LoginConcrete>();
builder.Services.AddScoped<IHome, HomeConcrete>();
builder.Services.AddScoped<IAdmin, AdminConcrete>();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
