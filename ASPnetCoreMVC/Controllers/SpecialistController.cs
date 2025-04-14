using ASPnetCoreMVC.Models;
using ASPnetCoreMVC.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPnetCoreMVC.Controllers
{
    public class SpecialistController : Controller
    {
        private readonly ApplicationDbContext _context;

       
        public SpecialistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specialist/Index
        public IActionResult Index()
        {
            var specialists = _context.Specialists
                                      .Select(s => new SpecialistViewModel
                                      {
                                          IdSpecialist = s.IdSpecialist,
                                          LastNameSpecialist = s.LastNameSpecialist,
                                          FirstNameSpecialist = s.FirstNameSpecialist,
                                          PatronymicSpecialist = s.PatronymicSpecialist,
                                          PhoneNumberSpecialist = s.PhoneNumberSpecialist,
                                          NamePosition = new PositionOfASpecialistViewModel
                                          {
                                              IdPosition = s.IdPositionNavigation.IdPosition,
                                              NamePosition = s.IdPositionNavigation.NamePosition
                                          },
                                          LoginUser = new UserViewModel
                                          {
                                              IdUser = s.IdUserNavigation.IdUser,
                                              LoginUser = s.IdUserNavigation.LoginUser
                                          }
                                      })
                                      .ToList();

            return View(specialists); 
        }



        // GET: Specialist/Create
        public IActionResult Create()
        {
            // Получаем список должностей и пользователей для выпадающих списков
            var positions = _context.PositionOfASpecialists.Select(p => new PositionOfASpecialistViewModel
            {
                IdPosition = p.IdPosition,
                NamePosition = p.NamePosition
            }).ToList();

            var users = _context.Users.Select(u => new UserViewModel
            {
                IdUser = u.IdUser,
                LoginUser = u.LoginUser
            }).ToList();

            // Передаем данные для выпадающих списков в представление
            ViewData["Positions"] = new SelectList(positions, "IdPosition", "NamePosition");
            ViewData["Users"] = new SelectList(users, "IdUser", "LoginUser");

            return View();
        }

        // POST: Specialist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialistViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Проверяем, что значения должности и пользователя не равны 0, перед сохранением
                    if (model.NamePosition?.IdPosition == 0 || model.LoginUser?.IdUser == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Не выбраны должность или пользователь.");
                        // Повторно заполняем ViewData для выпадающих списков
                        ViewData["Positions"] = new SelectList(_context.PositionOfASpecialists, "IdPosition", "NamePosition", model.NamePosition?.IdPosition);
                        ViewData["Users"] = new SelectList(_context.Users, "IdUser", "LoginUser", model.LoginUser?.IdUser);
                        return View(model); // Возвращаем представление с ошибкой
                    }

                    var specialist = new Specialist
                    {
                        LastNameSpecialist = model.LastNameSpecialist,
                        FirstNameSpecialist = model.FirstNameSpecialist,
                        PatronymicSpecialist = model.PatronymicSpecialist,
                        PhoneNumberSpecialist = model.PhoneNumberSpecialist,
                        IdPosition = model.NamePosition.IdPosition, // Устанавливаем выбранную должность
                        IdUser = model.LoginUser.IdUser // Устанавливаем выбранного пользователя
                    };

                    _context.Specialists.Add(specialist);
                    await _context.SaveChangesAsync(); // Асинхронное сохранение

                    return RedirectToAction(nameof(Index)); // Перенаправляем на страницу со списком специалистов
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, если она произошла
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении данных.");
                }
            }

            // Если модель не валидна или произошла ошибка, передаем обратно данные для выпадающих списков
            ViewData["Positions"] = new SelectList(_context.PositionOfASpecialists, "IdPosition", "NamePosition", model.NamePosition?.IdPosition);
            ViewData["Users"] = new SelectList(_context.Users, "IdUser", "LoginUser", model.LoginUser?.IdUser);

            return View(model); // Возвращаем представление с моделью
        }

    }
}
