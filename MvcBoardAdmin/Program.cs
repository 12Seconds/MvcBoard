using AspNetCore.Unobtrusive.Ajax;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 종속성 주입
builder.Services.AddSingleton<HomeService>();

builder.Services.AddTransient<MemberDataManager>();
builder.Services.AddSingleton<MemberService>();

// Html.AjaxBeginForm 사용을 위한 AspNetCore.Unobtrusive.Ajax 패키지 설치 및 설정 작업 (MVC 5 의 Ajax.BeginForm 대체)
builder.Services.AddUnobtrusiveAjax();

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

// Html.AjaxBeginForm 사용을 위한 AspNetCore.Unobtrusive.Ajax 패키지 설치 및 설정 작업 (MVC 5 의 Ajax.BeginForm 대체)
app.UseUnobtrusiveAjax();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
