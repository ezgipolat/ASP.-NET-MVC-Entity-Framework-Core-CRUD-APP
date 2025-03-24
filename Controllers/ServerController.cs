using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerInventoryApp.Data;
using ServerInventoryApp.Models;

namespace ServerInventoryApp.Controllers
{
    public class ServerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var servers = await _context.Servers.Include(s => s.Category).ToListAsync();
            
            foreach (var server in servers)
            {
                server.Name = EncryptionHelper.Decrypt(server.EncryptedName);
                server.IPAddress = EncryptionHelper.Decrypt(server.EncryptedIPAddress);
            }
            
            return View(servers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (server == null)
            {
                return NotFound();
            }

            server.Name = EncryptionHelper.Decrypt(server.EncryptedName);
            server.IPAddress = EncryptionHelper.Decrypt(server.EncryptedIPAddress);

            return View(server);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IPAddress,CategoryId")] Server server)
        {
            try
            {
                var existingServer = await _context.Servers
                    .FirstOrDefaultAsync(s => s.EncryptedIPAddress == EncryptionHelper.Encrypt(server.IPAddress));

                if (existingServer != null)
                {
                    ModelState.AddModelError("IPAddress", "Bu IP adresi zaten kaydedilmiş. Lütfen başka bir IP adresi girin.");
                    ViewBag.Categories = _context.Categories
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                        .ToList();
                    return View(server);
                }

                if (ModelState.IsValid)
                {
                    server.EncryptedName = EncryptionHelper.Encrypt(server.Name);
                    server.EncryptedIPAddress = EncryptionHelper.Encrypt(server.IPAddress);

                    _context.Add(server);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu: " + ex.Message);
            }

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(server);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            server.Name = EncryptionHelper.Decrypt(server.EncryptedName);
            server.IPAddress = EncryptionHelper.Decrypt(server.EncryptedIPAddress);

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(server);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IPAddress,CategoryId")] Server server)
        {
            if (id != server.Id)
            {
                return NotFound();
            }

            var existingServer = await _context.Servers
                .FirstOrDefaultAsync(s => s.EncryptedIPAddress == EncryptionHelper.Encrypt(server.IPAddress) && s.Id != server.Id);

            if (existingServer != null)
            {
                ModelState.AddModelError("IPAddress", "Bu IP adresi zaten kaydedilmiş. Lütfen başka bir IP adresi girin.");
                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(server);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    server.EncryptedName = EncryptionHelper.Encrypt(server.Name);
                    server.EncryptedIPAddress = EncryptionHelper.Encrypt(server.IPAddress);

                    _context.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.Id))
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

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(server);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (server == null)
            {
                return NotFound();
            }

            server.Name = EncryptionHelper.Decrypt(server.EncryptedName);
            server.IPAddress = EncryptionHelper.Decrypt(server.EncryptedIPAddress);

            return View(server);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var server = await _context.Servers.FindAsync(id);
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }
    }
}
