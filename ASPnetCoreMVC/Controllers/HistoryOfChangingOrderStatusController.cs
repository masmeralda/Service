using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPnetCoreMVC.Contexts;
using ASPnetCoreMVC.Models;

namespace ASPnetCoreMVC.Controllers
{
    public class HistoryOfChangingOrderStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoryOfChangingOrderStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HistoryOfChangingOrderStatus/Index
        public async Task<IActionResult> Index()
        {
            var histories = await _context.HistoryOfChangingOrderStatuses
                .Select(h => new HistoryOfChangingOrderStatusViewModel
                {
                    IdHistory = h.IdHistory,
                    StatusHistory = h.StatusHistory,
                    ChangedAt = h.ChangedAt,
                    IdOrder = h.IdOrder,
                    IdSpecialist = h.IdSpecialist
                })
                .ToListAsync();

            return View(histories);
        }

        // GET: HistoryOfChangingOrderStatus/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var history = await _context.HistoryOfChangingOrderStatuses
                .FirstOrDefaultAsync(h => h.IdHistory == id);
            if (history == null)
            {
                return NotFound();
            }

            // Используем ViewModel для отображения данных
            var historyViewModel = new HistoryOfChangingOrderStatusViewModel
            {
                IdHistory = history.IdHistory,
                StatusHistory = history.StatusHistory,
                ChangedAt = history.ChangedAt,
                IdOrder = history.IdOrder,
                IdSpecialist = history.IdSpecialist
            };
            return View(historyViewModel);
        }

        // POST: HistoryOfChangingOrderStatus/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var history = await _context.HistoryOfChangingOrderStatuses.FindAsync(id);
            if (history != null)
            {
                _context.HistoryOfChangingOrderStatuses.Remove(history);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



    }
}
