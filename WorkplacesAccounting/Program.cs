using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System;
using System.Reflection;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath="/Autorization/Index");

builder.Configuration.AddJsonFile("DataBase.json");

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
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/Autorization/Index", async (string? returnUrl, HttpContext context) =>
{
    // получаем из формы email и пароль
    var form = context.Request.Form;
    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
    if (!form.ContainsKey("username") || !form.ContainsKey("password"))
        return Results.BadRequest("login и/или пароль не установлены");

    string email = form["username"];
    string password = form["password"];

    // находим пользовател€ 
    //User? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    User? person = User.ConvertJsonToUser(await WorkplacesAccounting.Common.Moodle.Authenticate(email, password));
    // если пользователь не найден, отправл€ем статусный код 401
    if (person is null) return Results.BadRequest("Ќе авторизован");

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.username) };
    // создаем объект ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // установка аутентификационных куки
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl /*?? "/"*/);
});

app.Map("/loaddata", () => Data.LoadData());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorization}/{action=Index}/{id?}");

app.Run();
