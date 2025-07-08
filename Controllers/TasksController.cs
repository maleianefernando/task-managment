using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var tasks = _context.Tasks.Include(t => t.Worker);
            return View(await tasks.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name", task.WorkerId);
            ViewData["StatusOptions"] = new SelectList(new[] { "Pendente", "Em Progresso", "Concluída", "Cancelada" }, task.Status);
            ViewData["TypeOptions"] = new SelectList(new[] { "Normal", "Urgente", "Imediato", "Outro" }, task.Type);
            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name");
            // ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name");
            ViewData["StatusOptions"] = new SelectList(new[] { "Pendente", "Em Progresso", "Concluída", "Cancelada" });
            ViewData["TypeOptions"] = new SelectList(new[] { "Normal", "Urgente", "Imediato", "Outro" });
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkerId,Title,Deadline,Status,Type")] Models.Task task)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Get the Worker object by WorkerId
            var worker = await _context.Workers.FindAsync(task.WorkerId);

            if (worker == null)
            {
                                task.Worker = worker;
                // ModelState.AddModelError("WorkerId", "Funcionário inválido");
            }
            else
            {
                task.Worker = worker; // Popular a navegação
                // Console.WriteLine(@"Worker: "+ task.Worker.Id);
            }

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name", task.WorkerId);
            // ViewBag.Workers = new SelectList(_context.Workers, "Id", "Name", task.WorkerId);
            ViewData["StatusOptions"] = new SelectList(new[] { "Pendente", "Em Progresso", "Concluída", "Cancelada" }, task.Status);
            ViewData["TypeOptions"] = new SelectList(new[] { "Normal", "Urgente", "Imediato", "Outro" }, task.Type);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = id });

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name", task.WorkerId);
            ViewData["StatusOptions"] = new SelectList(new[] { "Pendente", "Em Progresso", "Concluída", "Cancelada" }, task.Status);
            ViewData["TypeOptions"] = new SelectList(new[] { "Desenvolvimento", "Teste", "Documentação", "Reunião", "Outro" }, task.Type);
            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkerId,Title,Deadline,Status,Type")] Models.Task task)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }


            if (id != task.Id)
            {
                return NotFound();
            }

            // Get the Worker object by WorkerId
            var worker = await _context.Workers.FindAsync(task.WorkerId);

            if (worker == null)
            {
                ModelState.AddModelError("WorkerId", "Funcionário inválido");
            }
            else
            {
                task.Worker = worker; // Popular a navegação
                // Console.WriteLine(@"Worker: "+ task.Worker.Id);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Name", task.WorkerId);
            ViewData["StatusOptions"] = new SelectList(new[] { "Pendente", "Em Progresso", "Concluída", "Cancelada" }, task.Status);
            ViewData["TypeOptions"] = new SelectList(new[] { "Desenvolvimento", "Teste", "Documentação", "Reunião", "Outro" }, task.Type);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}