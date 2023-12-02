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
    public class GroupeUtilisateursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupeUtilisateursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupeUtilisateurs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GroupesUtilisateurs.Include(g => g.Groupe).Include(g => g.Utilisateur);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GroupeUtilisateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GroupesUtilisateurs == null)
            {
                return NotFound();
            }

            var groupeUtilisateur = await _context.GroupesUtilisateurs
                .Include(g => g.Groupe)
                .Include(g => g.Utilisateur)
                .FirstOrDefaultAsync(m => m.GroupeId == id);
            if (groupeUtilisateur == null)
            {
                return NotFound();
            }

            return View(groupeUtilisateur);
        }

        // GET: GroupeUtilisateurs/Create
        public IActionResult Create()
        {
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "GroupeId", "GroupeId");
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email");
            return View();
        }

        // POST: GroupeUtilisateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupeId,UtilisateurId")] GroupeUtilisateur groupeUtilisateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupeUtilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "GroupeId", "GroupeId", groupeUtilisateur.GroupeId);
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", groupeUtilisateur.UtilisateurId);
            return View(groupeUtilisateur);
        }

        // GET: GroupeUtilisateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GroupesUtilisateurs == null)
            {
                return NotFound();
            }

            var groupeUtilisateur = await _context.GroupesUtilisateurs.FindAsync(id);
            if (groupeUtilisateur == null)
            {
                return NotFound();
            }
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "GroupeId", "GroupeId", groupeUtilisateur.GroupeId);
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", groupeUtilisateur.UtilisateurId);
            return View(groupeUtilisateur);
        }

        // POST: GroupeUtilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupeId,UtilisateurId")] GroupeUtilisateur groupeUtilisateur)
        {
            if (id != groupeUtilisateur.GroupeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupeUtilisateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupeUtilisateurExists(groupeUtilisateur.GroupeId))
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
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "GroupeId", "GroupeId", groupeUtilisateur.GroupeId);
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "Email", groupeUtilisateur.UtilisateurId);
            return View(groupeUtilisateur);
        }

        // GET: GroupeUtilisateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GroupesUtilisateurs == null)
            {
                return NotFound();
            }

            var groupeUtilisateur = await _context.GroupesUtilisateurs
                .Include(g => g.Groupe)
                .Include(g => g.Utilisateur)
                .FirstOrDefaultAsync(m => m.GroupeId == id);
            if (groupeUtilisateur == null)
            {
                return NotFound();
            }

            return View(groupeUtilisateur);
        }

        // POST: GroupeUtilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GroupesUtilisateurs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GroupesUtilisateurs'  is null.");
            }
            var groupeUtilisateur = await _context.GroupesUtilisateurs.FindAsync(id);
            if (groupeUtilisateur != null)
            {
                _context.GroupesUtilisateurs.Remove(groupeUtilisateur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupeUtilisateurExists(int id)
        {
            return (_context.GroupesUtilisateurs?.Any(e => e.GroupeId == id)).GetValueOrDefault();
        }
    }
}
