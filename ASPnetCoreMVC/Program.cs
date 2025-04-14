using ASPnetCoreMVC.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавьте строку подключения к PostgreSQL из appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации с использованием куков
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Перенаправление на страницу авторизации
        options.LogoutPath = "/Account/Logout"; // Перенаправление при выходе
    });


// Добавьте сервисы для MVC
builder.Services.AddControllersWithViews();

// Добавляем поддержку сессий
builder.Services.AddSession();

var app = builder.Build();

// Используйте middleware для обработки запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Использование аутентификации
app.UseAuthorization(); // Использование авторизации

app.UseSession(); // Обеспечиваем использование сессий

// Настройка маршрутизации
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/Index",
    defaults: new { controller = "Dashboard", action = "Index" });


app.Run();
