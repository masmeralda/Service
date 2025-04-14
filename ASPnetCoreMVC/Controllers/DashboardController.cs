// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Mvc;

namespace ASPnetCoreMVC.Controllers
{
    public class DashboardController : Controller
    {
        // Страница для отображения после логина в зависимости от роли
        public IActionResult Index()
        {
            // Получаем айди роли пользователя из сессии
            var userRoleId = HttpContext.Session.GetInt32("UserRoleId");

            // В зависимости от роли показываем нужную страницу
            if (userRoleId == 3)  // Администратор
            {
                return View("AdminDashboard"); // Страница для администратора
            }
            else if (userRoleId == 2)  // Инженер
            {
                return View("EngineerDashboard"); // Страница для инженера
            }
            else if (userRoleId == 1)  // Оператор
            {
                return View("OperatorDashboard"); // Страница для оператора
            }
            else
            {
                // Если роли нет или сессия не установлена, редирект на страницу логина
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
