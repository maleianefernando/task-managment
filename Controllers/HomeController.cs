using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystemMVC.Models;
using System.Diagnostics;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.WorkersCount = await _context.Workers.CountAsync();
            ViewBag.TasksCount = await _context.Tasks.CountAsync();
            ViewBag.PendingTasksCount = await _context.Tasks.CountAsync(t => t.Status == "Pendente");
            ViewBag.CompletedTasksCount = await _context.Tasks.CountAsync(t => t.Status == "Concluída");

            var recentTasks = await _context.Tasks
                .Include(t => t.Worker)
                .OrderByDescending(t => t.Id)
                .Take(5)
                .ToListAsync();

            return View(recentTasks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}