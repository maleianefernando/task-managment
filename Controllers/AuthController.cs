using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string workerId, string password)
        {
            if (string.IsNullOrEmpty(workerId) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "O id e senha são obrigatórios.";
                return View();
            }

            if (!int.TryParse(workerId, out int workerIdInt))
            {
                ViewBag.Error = "O id deve ser um número válido.";
                return View();
            }

            var user = await _context.Users
                .Include(u => u.Worker)
                .FirstOrDefaultAsync(u => u.WorkerId == workerIdInt && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Credenciais inválidas.";
                return View();
            }

            // Autenticação simples com Session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("WorkerId", user.WorkerId.ToString());
            HttpContext.Session.SetString("WorkerName", user.Worker.Name);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string workerName, string workerPosition, char workerGender)
        {
            if (ModelState.IsValid)
            {
                // Primeiro, criar o Worker
                var worker = new Worker
                {
                    Name = workerName,
                    Position = workerPosition,
                    Gender = workerGender
                };

                _context.Workers.Add(worker);
                await _context.SaveChangesAsync();

                // Depois, criar o User
                user.WorkerId = worker.Id;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}