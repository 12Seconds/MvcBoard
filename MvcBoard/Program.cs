global using MvcBoard.Models;
global using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext �߰�
// builder.Services.AddDbContext<MvcBoardDbContext>(); // TODO DbContext ����

// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBoardDbContext"))); // �ڽ���
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

// HTTPS -> HTTP ���� ó��
// app.UseHttpsRedirection(); 
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// TODO �� �˾ƺ� ��
/*
app.MapControllerRoute(
    name: "community",
    pattern: "Community/{action}/{category?}/{page?}", // �߰����� �Ű������� �ɼųη� ����
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
