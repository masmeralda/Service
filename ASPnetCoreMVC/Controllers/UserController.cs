using ASPnetCoreMVC.Contexts;
using ASPnetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPnetCoreMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .Select(u => new UserViewModel
                {
                    IdUser = u.IdUser,
                    LoginUser = u.LoginUser,
                    PasswordUser = u.PasswordUser,
                    IdRole = u.IdRole ?? 0,
                    RoleName = u.IdRoleNavigation != null ? u.IdRoleNavigation.RoleName : "Нет роли"
                })
                .ToListAsync();

            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            // Заполнение ролей для выпадающего списка
            var roles = _context.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "IdRole", "RoleName");

            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    LoginUser = model.LoginUser,
                    PasswordUser = model.PasswordUser,
                    IdRole = model.IdRole
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var roles = _context.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "IdRole", "RoleName");
            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                IdUser = user.IdUser,
                LoginUser = user.LoginUser,
                PasswordUser = user.PasswordUser,
                IdRole = user.IdRole ?? 0,
                RoleName = user.IdRoleNavigation?.RoleName ?? "Нет роли"
            };

            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "IdRole", "RoleName", user.IdRole);

            return View(userViewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            if (id != model.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Обновляем данные пользователя
                user.LoginUser = model.LoginUser ?? string.Empty;
                user.PasswordUser = model.PasswordUser ?? string.Empty;
                user.IdRole = model.IdRole;

                // Обновляем пользователя
                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Если форма невалидна, загружаем список ролей
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "IdRole", "RoleName", model.IdRole);

            return View(model);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                IdUser = user.IdUser,
                LoginUser = user.LoginUser,
                PasswordUser = user.PasswordUser,
                IdRole = user.IdRole ?? 0,
                RoleName = user.IdRoleNavigation?.RoleName ?? "Нет роли"
            };

            return View(userViewModel);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
