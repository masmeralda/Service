using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPnetCoreMVC.Contexts;
using ASPnetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPnetCoreMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Order/Index
        public async Task<IActionResult> Index()
        {
            // Загружаем данные из базы данных с явным соединением таблиц
            var orders = await _context.Orders
                .Join(
                    _context.Clients, // Соединение с таблицей клиентов
                    order => order.IdClient, // Локальный ключ
                    client => client.IdClient, // Внешний ключ
                    (order, client) => new { order, client }
                )
                .Join(
                    _context.Specialists, // Соединение с таблицей специалистов
                    oc => oc.order.IdSpecialist, // Локальный ключ
                    specialist => specialist.IdSpecialist, // Внешний ключ
                    (oc, specialist) => new { oc.order, oc.client, specialist }
                )
                .Join(
                    _context.Services, // Соединение с таблицей услуг
                    ocs => ocs.order.IdService, // Локальный ключ
                    service => service.IdService, // Внешний ключ
                    (ocs, service) => new { ocs.order, ocs.client, ocs.specialist, service }
                )
                .Join(
                    _context.Technics, // Соединение с таблицей техники
                    ocss => ocss.order.IdTechnic, // Локальный ключ
                    technic => technic.IdTechnic, // Внешний ключ
                    (ocss, technic) => new OrderViewModel
                    {
                        IdOrder = ocss.order.IdOrder,
                        StatusOrder = ocss.order.StatusOrder,
                        CreationDateOrder = ocss.order.CreationDateOrder,
                        CompletionDateOrder = ocss.order.CompletionDateOrder,
                        CommentOrder = ocss.order.CommentOrder,

                        Client = new ClientViewModel // Преобразование в ClientViewModel
                        {
                            LastNameClient = ocss.client.LastNameClient,
                            FirstNameClient = ocss.client.FirstNameClient,
                            PatronymicClient = ocss.client.PatronymicClient,
                            PhoneClient = ocss.client.PhoneClient,
                            EmailClient = ocss.client.EmailClient
                        },
                        ClientId = ocss.client.IdClient, // Присваиваем только идентификатор клиента

                        Specialist = new SpecialistViewModel // Преобразование в SpecialistViewModel
                        {
                            IdSpecialist = ocss.specialist.IdSpecialist,
                            LastNameSpecialist = ocss.specialist.LastNameSpecialist,
                            FirstNameSpecialist = ocss.specialist.FirstNameSpecialist,
                            PatronymicSpecialist = ocss.specialist.PatronymicSpecialist,
                            PhoneNumberSpecialist = ocss.specialist.PhoneNumberSpecialist
                        },

                        ServiceId = ocss.order.IdService,

                        // Преобразование в ServiceViewModel
                        Service = new ServiceViewModel
                        {
                            IdService = ocss.service.IdService,
                            NameService = ocss.service.NameService,
                            PriceService = ocss.service.PriceService,
                            ExecutionTimeService = ocss.service.ExecutionTimeService
                        },

                        TechnicId = ocss.order.IdTechnic,

                        // Преобразование в TechnicViewModel
                        Technic = new TechnicViewModel
                        {
                            DescriptionTechnic = technic.DescriptionTechnic,
                            PhotoTechnic = technic.PhotoTechnic
                        }
                    }
                )
                .ToListAsync();

            return View(orders); // Передаём список OrderViewModel в представление
        }


        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["ClientName"] = new SelectList(
                _context.Clients.Select(client => new
                {
                    IdClient = client.IdClient,
                    FullName = $"{client.LastNameClient} {client.FirstNameClient} {client.PatronymicClient}"
                }),
                "IdClient", "FullName"
            );

            // Добавляем полный имя специалиста (Фамилия Имя Отчество)
            ViewData["SpecialistName"] = new SelectList(
                _context.Specialists.Select(specialist => new
                {
                    IdSpecialist = specialist.IdSpecialist,
                    FullName = $"{specialist.LastNameSpecialist} {specialist.FirstNameSpecialist} {specialist.PatronymicSpecialist}"
                }),
                "IdSpecialist", "FullName"
            );

            ViewData["TechnicName"] = new SelectList(_context.Technics, "IdTechnic", "DescriptionTechnic");
            ViewData["ServiceName"] = new SelectList(_context.Services, "IdService", "NameService");

            return View();
        }


        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusOrder,CreationDateOrder,CompletionDateOrder,CommentOrder,ClientId,SpecialistId,TechnicId,ServiceId")] OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                // Получение данных для создания заказа
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.IdClient == orderViewModel.ClientId);
                var specialist = await _context.Specialists.FirstOrDefaultAsync(s => s.IdSpecialist == orderViewModel.SpecialistId);
                var technic = await _context.Technics.FirstOrDefaultAsync(t => t.IdTechnic == orderViewModel.TechnicId);
                var service = await _context.Services.FirstOrDefaultAsync(s => s.IdService == orderViewModel.ServiceId);
                // Создание нового объекта Order
                var order = new Order
                {
                    StatusOrder = orderViewModel.StatusOrder,
                    CreationDateOrder = orderViewModel.CreationDateOrder,
                    CompletionDateOrder = orderViewModel.CompletionDateOrder,
                    CommentOrder = orderViewModel.CommentOrder,
                    IdClient = orderViewModel.ClientId,
                    IdSpecialist = orderViewModel.SpecialistId,
                    IdTechnic = orderViewModel.TechnicId,
                    IdService = orderViewModel.ServiceId
                };
                // Добавление в базу данных
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Перенаправление на страницу Index после успешного создания заказа
            }
            // Если модель не прошла валидацию, возвращаем выпадающие списки
            ViewData["ClientName"] = new SelectList(_context.Clients, "IdClient", "LastNameClient", orderViewModel.ClientId);
            ViewData["SpecialistName"] = new SelectList(_context.Specialists, "IdSpecialist", "IdSpecialist", orderViewModel.SpecialistId);
            ViewData["TechnicName"] = new SelectList(_context.Technics, "IdTechnic", "DescriptionTechnic", orderViewModel.TechnicId);
            ViewData["ServiceName"] = new SelectList(_context.Services, "IdService", "IdService", orderViewModel.ServiceId);
            return View(orderViewModel); // Возвращаем форму с ошибками валидации
        }
        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }
            // Используем ViewModel для отображения данных
            var orderViewModel = new OrderViewModel
            {
                IdOrder = order.IdOrder,
                StatusOrder = order.StatusOrder,
                CreationDateOrder = order.CreationDateOrder,
                CompletionDateOrder = order.CompletionDateOrder,
                CommentOrder = order.CommentOrder
            };
            return View(orderViewModel);
        }
        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Ищем заказ по идентификатору
            var order = await _context.Orders
                .Include(o => o.IdClientNavigation)
                .Include(o => o.IdServiceNavigation)
                .Include(o => o.IdSpecialistNavigation)
                .Include(o => o.IdTechnicNavigation)
                .FirstOrDefaultAsync(o => o.IdOrder == id);

            // Если заказ не найден, возвращаем NotFound
            if (order == null)
            {
                return NotFound();
            }

            // Создаем ViewModel для отображения
            var orderViewModel = new OrderViewModel
            {
                IdOrder = order.IdOrder,
                StatusOrder = order.StatusOrder,
                CreationDateOrder = order.CreationDateOrder,
                CompletionDateOrder = order.CompletionDateOrder,
                CommentOrder = order.CommentOrder,

                Client = new ClientViewModel
                {
                    LastNameClient = order.IdClientNavigation.LastNameClient,
                    FirstNameClient = order.IdClientNavigation.FirstNameClient,
                    PatronymicClient = order.IdClientNavigation.PatronymicClient,
                    PhoneClient = order.IdClientNavigation.PhoneClient,
                    EmailClient = order.IdClientNavigation.EmailClient,
                    StreetClient = order.IdClientNavigation.StreetClient,
                    HouseClient = order.IdClientNavigation.HouseClient,
                    ApartmentNumberClient = order.IdClientNavigation.ApartmentNumberClient
                },

                Specialist = new SpecialistViewModel
                {
                    LastNameSpecialist = order.IdSpecialistNavigation.LastNameSpecialist,
                    FirstNameSpecialist = order.IdSpecialistNavigation.FirstNameSpecialist,
                    PatronymicSpecialist = order.IdSpecialistNavigation.PatronymicSpecialist,
                    PhoneNumberSpecialist = order.IdSpecialistNavigation.PhoneNumberSpecialist
                },

                Service = new ServiceViewModel
                {
                    NameService = order.IdServiceNavigation.NameService,
                    PriceService = order.IdServiceNavigation.PriceService,
                    ExecutionTimeService = order.IdServiceNavigation.ExecutionTimeService
                },

                Technic = new TechnicViewModel
                {
                    DescriptionTechnic = order.IdTechnicNavigation.DescriptionTechnic,
                    PhotoTechnic = order.IdTechnicNavigation.PhotoTechnic
                }
            };

            // Передаем в представление объект ViewModel
            return View(orderViewModel);
        }



        // ИНЖЕНЕР

        // GET: Order/Index
        public async Task<IActionResult> Engineer_Index(string status)
        {
            ViewBag.StatusFilter = status;  // Добавляем status в ViewBag

            var ordersQuery = _context.Orders
                .Join(
                    _context.Clients,
                    order => order.IdClient,
                    client => client.IdClient,
                    (order, client) => new { order, client }
                )
                .Join(
                    _context.Specialists,
                    oc => oc.order.IdSpecialist,
                    specialist => specialist.IdSpecialist,
                    (oc, specialist) => new { oc.order, oc.client, specialist }
                )
                .Join(
                    _context.Services,
                    ocs => ocs.order.IdService,
                    service => service.IdService,
                    (ocs, service) => new { ocs.order, ocs.client, ocs.specialist, service }
                )
                .Join(
                    _context.Technics,
                    ocss => ocss.order.IdTechnic,
                    technic => technic.IdTechnic,
                    (ocss, technic) => new OrderViewModel
                    {
                        IdOrder = ocss.order.IdOrder,
                        StatusOrder = ocss.order.StatusOrder,
                        CreationDateOrder = ocss.order.CreationDateOrder,
                        CompletionDateOrder = ocss.order.CompletionDateOrder,
                        CommentOrder = ocss.order.CommentOrder,
                        Client = new ClientViewModel
                        {
                            LastNameClient = ocss.client.LastNameClient,
                            FirstNameClient = ocss.client.FirstNameClient,
                            PatronymicClient = ocss.client.PatronymicClient,
                            PhoneClient = ocss.client.PhoneClient,
                            EmailClient = ocss.client.EmailClient
                        },
                        ClientId = ocss.client.IdClient,
                        Specialist = new SpecialistViewModel
                        {
                            IdSpecialist = ocss.specialist.IdSpecialist,
                            LastNameSpecialist = ocss.specialist.LastNameSpecialist,
                            FirstNameSpecialist = ocss.specialist.FirstNameSpecialist,
                            PatronymicSpecialist = ocss.specialist.PatronymicSpecialist,
                            PhoneNumberSpecialist = ocss.specialist.PhoneNumberSpecialist
                        },
                        ServiceId = ocss.order.IdService,
                        Service = new ServiceViewModel
                        {
                            IdService = ocss.service.IdService,
                            NameService = ocss.service.NameService,
                            PriceService = ocss.service.PriceService,
                            ExecutionTimeService = ocss.service.ExecutionTimeService
                        },
                        TechnicId = ocss.order.IdTechnic,
                        Technic = new TechnicViewModel
                        {
                            DescriptionTechnic = technic.DescriptionTechnic,
                            PhotoTechnic = technic.PhotoTechnic
                        }
                    }
                );

            // Применение фильтрации по статусу, если оно задано
            if (!string.IsNullOrEmpty(status))
            {
                ordersQuery = ordersQuery.Where(o => o.StatusOrder == status);
            }

            var orders = await ordersQuery.ToListAsync();
            return View(orders);
        }



        [HttpGet]
        public IActionResult Engineer_Edit(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.IdOrder == id);
            if (order == null)
            {
                return NotFound();
            }

            // Конвертируем Order в OrderViewModel
            var orderViewModel = new OrderViewModel
            {
                IdOrder = order.IdOrder,
                StatusOrder = order.StatusOrder,
                // Другие необходимые поля (если они есть)
            };

            // Передаем в представление объект типа OrderViewModel
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Engineer_Edit(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders.FirstOrDefault(o => o.IdOrder == model.IdOrder);
                if (order != null)
                {
                    // Проверяем, изменился ли статус с "Завершен"
                    if (order.StatusOrder == "Завершен" && model.StatusOrder != "Завершен")
                    {
                        // Если статус изменился с "Завершен", удаляем дату завершения
                        order.CompletionDateOrder = null;
                    }

                    // Обновляем только статус заказа
                    order.StatusOrder = model.StatusOrder;

                    // Если статус "Завершен", устанавливаем сегодняшнюю дату как дату завершения
                    if (model.StatusOrder == "Завершен")
                    {
                        order.CompletionDateOrder = DateTime.Now; // Устанавливаем текущую дату
                    }

                    // Сохраняем изменения в базе данных
                    _context.SaveChanges();
                    return RedirectToAction("Engineer_Index"); // Перенаправление на страницу списка заказов
                }
            }

            return View(model);
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.IdOrder == id);
        }

    }
}
