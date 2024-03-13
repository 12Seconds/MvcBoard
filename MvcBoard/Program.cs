global using MvcBoard.Models;
global using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MvcBoard.Managers;
using MvcBoard.Services;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddTransient<IWebHostEnvironment>(); // 이건 할필요 X 하면 오류남 -> 싱글톤으로 해야 했나? 일단은 안해줘도 쓸 수 있으니까

// TODO 작업중
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // 이걸까?

// TODO Transient, Singleton, Scope 중 뭐가 맞을까
builder.Services.AddTransient<CommunityDataManagers>(); 
builder.Services.AddTransient<CommunityService>();

// DbContext 추가
// builder.Services.AddDbContext<MvcBoardDbContext>(); // TODO DbContext 제거

// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBoardDbContext"))); // 자습서
// var conString = builder.Configuration.GetConnectionString("MvcBoardDbContext");
// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(conString));

var app = builder.Build();

// TODO 지울 것 (또는 로그 모듈 붙이면 그걸로 변경)
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

// HTTPS -> HTTP 변경 처리
// app.UseHttpsRedirection(); 
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// TODO 더 알아볼 것
/*
app.MapControllerRoute(
    name: "community",
    pattern: "Community/{action}/{category?}/{page?}", // 추가적인 매개변수를 옵셔널로 지정
    defaults: new { controller = "Community", action = "Index" });
*/
app.MapControllerRoute(
    name: "community",
    pattern: "Community/{action}/{id?}",
    defaults: new { controller = "Community", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
