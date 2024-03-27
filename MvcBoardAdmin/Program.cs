using AspNetCore.Unobtrusive.Ajax;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
