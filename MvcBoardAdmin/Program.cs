using AspNetCore.Unobtrusive.Ajax;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.JWT;
using MvcBoardAdmin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ���Ӽ� ����
// builder.Services.AddSingleton<Utility>();

// builder.Services.AddSingleton<JWTManager>();

builder.Services.AddTransient<AuthorityDataManager>();

builder.Services.AddTransient<AdmUserDataManager>();
builder.Services.AddSingleton<HomeService>();

builder.Services.AddTransient<MemberDataManager>();
builder.Services.AddSingleton<MemberService>();

builder.Services.AddTransient<BoardDataManager>();
builder.Services.AddSingleton<BoardService>();

builder.Services.AddTransient<PostDataManager>();
builder.Services.AddSingleton<PostService>();

builder.Services.AddTransient<CommentDataManager>();
builder.Services.AddSingleton<CommentService>();

// Html.AjaxBeginForm ����� ���� AspNetCore.Unobtrusive.Ajax ��Ű�� ��ġ �� ���� �۾� (MVC 5 �� Ajax.BeginForm ��ü)
builder.Services.AddUnobtrusiveAjax();

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

// Html.AjaxBeginForm ����� ���� AspNetCore.Unobtrusive.Ajax ��Ű�� ��ġ �� ���� �۾� (MVC 5 �� Ajax.BeginForm ��ü)
app.UseUnobtrusiveAjax();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
