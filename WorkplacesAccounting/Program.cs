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
    // �������� �� ����� email � ������
    var form = context.Request.Form;
    // ���� email �/��� ������ �� �����������, �������� ��������� ��� ������ 400
    if (!form.ContainsKey("username") || !form.ContainsKey("password"))
        return Results.BadRequest("login �/��� ������ �� �����������");

    string email = form["username"];
    string password = form["password"];

    // ������� ������������ 
    //User? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    User? person = User.ConvertJsonToUser(await WorkplacesAccounting.Common.Moodle.Authenticate(email, password));
    // ���� ������������ �� ������, ���������� ��������� ��� 401
    if (person is null) return Results.BadRequest("�� �����������");

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.username) };
    // ������� ������ ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // ��������� ������������������ ����
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl /*?? "/"*/);
});

app.Map("/loaddata", () => Data.LoadData());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorization}/{action=Index}/{id?}");

app.Run();
