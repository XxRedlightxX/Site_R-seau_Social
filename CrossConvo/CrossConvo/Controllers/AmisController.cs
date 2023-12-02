using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrossConvo.Models;

namespace CrossConvoApp.Controllers
{
    public class AmisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AmisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Amis
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Amis.Include(a => a.Utilisateur);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Amis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Amis == null)
            {
                return NotFound();
            }

            var ami = await _context.Amis
                .Include(a => a.Utilisateur)
                .FirstOrDefaultAsync(m => m.idAmi == id);
            if (ami == null)
            {
                return NotFound();
            }

            return View(ami);
        }

        // GET: Amis/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email");
            return View();
        }

        // POST: Amis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idAmi,Nom,Prenom,Username,Email,UtilisateurId")] Ami ami)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ami);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", ami.UtilisateurId);
            return View(ami);
        }

        // GET: Amis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Amis == null)
            {
                return NotFound();
            }

            var ami = await _context.Amis.FindAsync(id);
            if (ami == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", ami.UtilisateurId);
            return View(ami);
        }

        // POST: Amis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idAmi,Nom,Prenom,Username,Email,UtilisateurId")] Ami ami)
        {
            if (id != ami.idAmi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ami);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmiExists(ami.idAmi))
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
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", ami.UtilisateurId);
            return View(ami);
        }

        // GET: Amis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Amis == null)
            {
                return NotFound();
            }

            var ami = await _context.Amis
                .Include(a => a.Utilisateur)
                .FirstOrDefaultAsync(m => m.idAmi == id);
            if (ami == null)
            {
                return NotFound();
            }

            return View(ami);
        }

        // POST: Amis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Amis == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Amis'  is null.");
            }
            var ami = await _context.Amis.FindAsync(id);
            if (ami != null)
            {
                _context.Amis.Remove(ami);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmiExists(int id)
        {
            return (_context.Amis?.Any(e => e.idAmi == id)).GetValueOrDefault();
        }
    }
}
