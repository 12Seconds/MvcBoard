global using MvcBoard.Models;
global using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MvcBoard.Managers;
using MvcBoard.Services;
using AspNetCore.Unobtrusive.Ajax;
using MvcBoard.Utills;
using MvcBoard.Managers.JWT;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddTransient<IWebHostEnvironment>(); // �̰� ���ʿ� X �ϸ� ������ -> �̱������� �ؾ� �߳�? �ϴ��� �����൵ �� �� �����ϱ�

// TODO �۾���
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // �̰ɱ�?

// TODO Transient, Singleton, Scope �� ���� ������
builder.Services.AddSingleton<JWTManager>();
builder.Services.AddSingleton<Utillity>();

builder.Services.AddTransient<CommunityDataManagers>(); 
builder.Services.AddTransient<CommunityService>();

builder.Services.AddTransient<UserDataManager>();
builder.Services.AddTransient<UserService>();

// Html.AjaxBeginForm ����� ���� AspNetCore.Unobtrusive.Ajax ��Ű�� ��ġ �� ���� �۾� (MVC 5 �� Ajax.BeginForm ��ü)
builder.Services.AddUnobtrusiveAjax();

// ���� ��� ����
/*
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});
*/

// DbContext �߰�
// builder.Services.AddDbContext<MvcBoardDbContext>(); // TODO DbContext ����

// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBoardDbContext"))); // �ڽ���
// var conString = builder.Configuration.GetConnectionString("MvcBoardDbContext");
// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(conString));

var app = builder.Build();

// TODO ���� �� (�Ǵ� �α� ��� ���̸� �װɷ� ����)
Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
Console.WriteLine($"## Program.cs ## app.Environment.IsDevelopment(): {app.Environment.IsDevelopment()}, app.Environment.IsProduction(): {app.Environment.IsProduction()}");
Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// HTTPS -> HTTP ���� ó��
// app.UseHttpsRedirection(); 
app.UseStaticFiles();

// Html.AjaxBeginForm ����� ���� AspNetCore.Unobtrusive.Ajax ��Ű�� ��ġ �� ���� �۾� (MVC 5 �� Ajax.BeginForm ��ü)
app.UseUnobtrusiveAjax();

app.UseRouting();

app.UseAuthorization();

// ���� ��� ����
// app.UseSession();

// TODO �� �˾ƺ� ��
/*
app.MapControllerRoute(
    name: "community",
    pattern: "Community/{action}/{category?}/{page?}", // �߰����� �Ű������� �ɼųη� ����
    defaults: new { controller = "Community", action = "Index" });
*/
/*
app.MapControllerRoute(
    name: "login",
    pattern: "login/{id?}",
    defaults: new { controller = "User", action = "Login" });
*/

app.MapControllerRoute(
    name: "login",
    pattern: "Login/{id?}",
    defaults: new { controller = "User", action = "Index" });

app.MapControllerRoute(
    name: "user",
    pattern: "User/{action}/{id?}",
    defaults: new { controller = "User", action = "Index" });

app.MapControllerRoute(
    name: "community",
    pattern: "Community/{action}/{id?}",
    defaults: new { controller = "Community", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
