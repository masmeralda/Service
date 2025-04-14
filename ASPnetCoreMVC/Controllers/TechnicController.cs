using Microsoft.AspNetCore.Mvc;
using ASPnetCoreMVC.Models;
using ASPnetCoreMVC.Contexts;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ASPnetCoreMVC.Controllers
{
    public class TechnicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TechnicController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            // Загружаем данные из базы данных
            var technics = _context.Technics
                .Join(
                    _context.ModelTechnics, // Соединяем с таблицей ModelTechnics
                    technic => technic.IdModelTechnic, // Локальный ключ (Technics)
                    model => model.IdModelTechnic,    // Внешний ключ (ModelTechnics)
                    (technic, model) => new TechnicViewModel
                    {
                        IdTechnic = technic.IdTechnic,
                        DescriptionTechnic = technic.DescriptionTechnic,
                        PhotoTechnic = technic.PhotoTechnic,
                        IdModelTechnic = technic.IdModelTechnic ?? 0,
                        ModelTechnic = new ModelTechnicViewModel
                        {
                            IdModelTechnic = model.IdModelTechnic,
                            NameModelTechnic = model.NameModelTechnic
                        }
                    })
                .ToList();

            return View(technics); // Передаем данные в представление
        }

        // GET: Technic/Create
        public IActionResult Create()
        {
            // Получаем все типы техники для первого списка
            var types = _context.TypeTechnics.ToList();
            ViewBag.Types = types;

            // Получаем все модели для первого типа
            var models = _context.ModelTechnics
                .Where(m => m.IdTypeTechnic == types.FirstOrDefault().IdTypeTechnic) // Фильтруем модели по первому типу
                .ToList();
            ViewBag.Models = models;

            return View();
        }

        // POST: Technic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechnicViewModel model, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                // Обработка фото, если оно есть
                if (photo != null && photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        byte[] photoBytes = memoryStream.ToArray();

                        // Преобразование в строку Base64
                        model.PhotoTechnic = Convert.ToBase64String(photoBytes);
                    }
                }

                // Создаем новую запись в таблице Technic
                var technic = new Technic
                {
                    DescriptionTechnic = model.DescriptionTechnic,
                    PhotoTechnic = model.PhotoTechnic,  // Сохраняем строку Base64
                    IdModelTechnic = model.IdModelTechnic
                };

                // Добавляем и сохраняем в базу данных
                _context.Add(technic);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));  // Перенаправление на список
            }

            // Если модель не прошла валидацию, повторно отображаем форму с ошибками
            var types = _context.TypeTechnics.ToList();
            ViewBag.Types = types;
            return View(model);
        }

        // Получаем модели по выбранному типу техники
        public IActionResult GetModelsByType(int idTypeTechnic)
        {
            var models = _context.ModelTechnics
                .Where(m => m.IdTypeTechnic == idTypeTechnic)
                .Select(m => new
                {
                    m.IdModelTechnic,
                    m.NameModelTechnic
                })
                .ToList();

            return Json(models);
        }

        // GET: Technic/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var technic = await _context.Technics
                .Include(t => t.IdModelTechnicNavigation) // Подключаем связанные данные, если необходимо
                .FirstOrDefaultAsync(t => t.IdTechnic == id);

            if (technic == null)
            {
                return NotFound();
            }

            // Создаем ViewModel для отображения информации
            var technicViewModel = new TechnicViewModel
            {
                IdTechnic = technic.IdTechnic,
                DescriptionTechnic = technic.DescriptionTechnic,
                PhotoTechnic = technic.PhotoTechnic,
                IdModelTechnic = technic.IdModelTechnic ?? 0
            };

            return View(technicViewModel);
        }

        // POST: Technic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var technic = await _context.Technics.FindAsync(id);
            if (technic != null)
            {
                // Удаляем фото, если оно есть
                if (!string.IsNullOrEmpty(technic.PhotoTechnic))
                {
                    var photoPath = Path.Combine(_env.WebRootPath, "uploads", technic.PhotoTechnic);
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                _context.Technics.Remove(technic);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Technic/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var technic = await _context.Technics
                .Include(t => t.IdModelTechnicNavigation)
                .FirstOrDefaultAsync(t => t.IdTechnic == id);

            if (technic == null)
            {
                return NotFound();
            }

            var types = _context.TypeTechnics.ToList();
            ViewBag.Types = types;

            var models = _context.ModelTechnics
                .Where(m => m.IdTypeTechnic == technic.IdModelTechnicNavigation.IdTypeTechnic)
                .ToList();
            ViewBag.Models = models;

            var technicViewModel = new TechnicViewModel
            {
                IdTechnic = technic.IdTechnic,
                DescriptionTechnic = technic.DescriptionTechnic,
                PhotoTechnic = technic.PhotoTechnic,
                IdModelTechnic = technic.IdModelTechnic ?? 0
            };

            return View(technicViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TechnicViewModel model, IFormFile photo)
        {
            if (id != model.IdTechnic)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var technic = await _context.Technics.FindAsync(id);
                if (technic == null)
                {
                    return NotFound();
                }

                technic.DescriptionTechnic = model.DescriptionTechnic;

                // Если новое фото не загружено, оставляем текущее
                if (photo != null && photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        byte[] photoBytes = memoryStream.ToArray();
                        technic.PhotoTechnic = Convert.ToBase64String(photoBytes); // Сохраняем новое фото
                    }
                }
                else
                {
                    // Если фото не было загружено, оставляем текущее
                    technic.PhotoTechnic = technic.PhotoTechnic;
                }

                technic.IdModelTechnic = model.IdModelTechnic;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Technics.Any(e => e.IdTechnic == model.IdTechnic))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var types = _context.TypeTechnics.ToList();
            ViewBag.Types = types;

            var models = _context.ModelTechnics
                .Where(m => m.IdTypeTechnic == model.IdModelTechnic)
                .ToList();
            ViewBag.Models = models;

            return View(model);
        }




    }
}
