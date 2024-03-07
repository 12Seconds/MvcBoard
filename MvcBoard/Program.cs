global using MvcBoard.Models;
global using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext �߰�
builder.Services.AddDbContext<MvcBoardDbContext>();
// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcBoardDbContext"))); // �ڽ���
// var conString = builder.Configuration.GetConnectionString("MvcBoardDbContext");
// builder.Services.AddDbContext<MvcBoardDbContext>(options => options.UseSqlServer(conString));


// TODO ���� �� (��ĳ���� ��ɾ�)
// Scaffold-DbContext -Connection "Server=DESKTOP-5AFMG8G;Database=MVC_BOARD_DB;Trusted_Connection=true;TrustServerCertificate=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

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
