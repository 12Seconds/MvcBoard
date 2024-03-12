global using MvcBoard.Models;
global using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext 추가
// builder.Services.AddDbContext<MvcBoardDbContext>(); // TODO DbContext 제거

// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBoardDbContext"))); // 자습서
// var conString = builder.Configuration.GetConnectionString("MvcBoardDbContext");
// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(conString));

var app = builder.Build();

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
