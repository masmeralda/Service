using ASPnetCoreMVC.Contexts;
using ASPnetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPnetCoreMVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;
            var clientsQuery = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var searchTerms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    var lowerTerm = term.ToLower(); 
                    clientsQuery = clientsQuery.Where(c =>
                        c.LastNameClient.ToLower().Contains(lowerTerm) ||
                        c.FirstNameClient.ToLower().Contains(lowerTerm) ||
                        c.PatronymicClient.ToLower().Contains(lowerTerm));
                }
            }
            var clients = await clientsQuery
                .Select(c => new ClientViewModel
                {
                    IdClient = c.IdClient,
                    LastNameClient = c.LastNameClient,
                    FirstNameClient = c.FirstNameClient,
                    PatronymicClient = c.PatronymicClient,
                    PhoneClient = c.PhoneClient,
                    EmailClient = c.EmailClient,
                    StreetClient = c.StreetClient,
                    HouseClient = c.HouseClient,
                    ApartmentNumberClient = c.ApartmentNumberClient
                })
                .ToListAsync();
            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    LastNameClient = clientViewModel.LastNameClient,
                    FirstNameClient = clientViewModel.FirstNameClient,
                    PatronymicClient = clientViewModel.PatronymicClient,
                    PhoneClient = clientViewModel.PhoneClient,
                    EmailClient = clientViewModel.EmailClient,
                    StreetClient = clientViewModel.StreetClient,
                    HouseClient = clientViewModel.HouseClient,
                    ApartmentNumberClient = clientViewModel.ApartmentNumberClient
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); 
            }

            return View(clientViewModel); 
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.IdClient == id);
            if (client == null)
            {
                return NotFound();
            }
            var clientViewModel = new ClientViewModel
            {
                IdClient = client.IdClient,
                LastNameClient = client.LastNameClient,
                FirstNameClient = client.FirstNameClient,
                PatronymicClient = client.PatronymicClient,
                PhoneClient = client.PhoneClient,
                EmailClient = client.EmailClient,
                StreetClient = client.StreetClient,
                HouseClient = client.HouseClient,
                ApartmentNumberClient = client.ApartmentNumberClient
            };
            return View(clientViewModel);
        }
        // POST: Client/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            // Создаем ViewModel для отображения данных
            var clientViewModel = new ClientViewModel
            {
                IdClient = client.IdClient,
                LastNameClient = client.LastNameClient,
                FirstNameClient = client.FirstNameClient,
                PatronymicClient = client.PatronymicClient,
                PhoneClient = client.PhoneClient,
                EmailClient = client.EmailClient,
                StreetClient = client.StreetClient,
                HouseClient = client.HouseClient,
                ApartmentNumberClient = client.ApartmentNumberClient
            };

            return View(clientViewModel);
        }
        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel clientViewModel)
        {
            if (id != clientViewModel.IdClient)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                // Обновляем данные клиента
                client.LastNameClient = clientViewModel.LastNameClient;
                client.FirstNameClient = clientViewModel.FirstNameClient;
                client.PatronymicClient = clientViewModel.PatronymicClient;
                client.PhoneClient = clientViewModel.PhoneClient;
                client.EmailClient = clientViewModel.EmailClient;
                client.StreetClient = clientViewModel.StreetClient;
                client.HouseClient = clientViewModel.HouseClient;
                client.ApartmentNumberClient = clientViewModel.ApartmentNumberClient;

                _context.Update(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(clientViewModel);
        }
    }
}
