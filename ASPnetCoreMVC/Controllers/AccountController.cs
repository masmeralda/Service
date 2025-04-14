using ASPnetCoreMVC.Contexts;
using ASPnetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace ASPnetCoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Страница входа
        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        // Обработка логина
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Проверяем, есть ли пользователь с таким логином и паролем
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.LoginUser == model.LoginUser && u.PasswordUser == model.PasswordUser);

                if (user != null)
                {
                    // Получаем айди роли пользователя
                    var userRoleId = user.IdRole;

                    // Сохраняем айди роли и имя пользователя в сессии
                    HttpContext.Session.SetInt32("UserRoleId", userRoleId.Value);  // Сохраняем роль как айди
                    HttpContext.Session.SetString("UserName", user.LoginUser); // Сохраняем имя пользователя

                    // Перенаправляем на панель пользователя
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    // Неверный логин или пароль
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                }
            }

            return View(model);
        }

        // Выход
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Удаляем данные сессии
            HttpContext.Session.Clear();

            // Удаляем аутентификационные куки
            await HttpContext.SignOutAsync("CookieAuth");

            // Перенаправляем на страницу авторизации
            return RedirectToAction("Login", "Account");
        }
    }
}
