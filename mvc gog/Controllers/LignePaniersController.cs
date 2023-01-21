using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_gog.Data;
using mvc_gog.Models;

namespace mvc_gog.Controllers
{
    [Authorize]
    public class LignePaniersController : Controller
    {
        private readonly mvc_gogContext _context;

        public LignePaniersController(mvc_gogContext context)
        {
            _context = context;
        }

        // GET: LignePaniers
        public async Task<IActionResult> Index()
        {
            var mvc_gogContext = _context.LignePanier.Include(l => l.panier).Include(l => l.produit);
            return View(await mvc_gogContext.ToListAsync());
        }

        // GET: LignePaniers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LignePanier == null)
            {
                return NotFound();
            }

            var lignePanier = await _context.LignePanier
                .Include(l => l.panier)
                .Include(l => l.produit)
                .FirstOrDefaultAsync(m => m.LignePanierID == id);
            if (lignePanier == null)
            {
                return NotFound();
            }

            return View(lignePanier);
        }

        // GET: LignePaniers/Create
        public IActionResult Create()
        {
            ViewData["PanierID"] = new SelectList(_context.Panier, "PanierID", "PanierID");
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID");
            return View();
        }

        // POST: LignePaniers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LignePanierID,PanierID,ProduitID,Qte")] LignePanier lignePanier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lignePanier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PanierID"] = new SelectList(_context.Panier, "PanierID", "PanierID", lignePanier.PanierID);
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", lignePanier.ProduitID);
            return View(lignePanier);
        }

        // GET: LignePaniers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LignePanier == null)
            {
                return NotFound();
            }

            var lignePanier = await _context.LignePanier.FindAsync(id);
            if (lignePanier == null)
            {
                return NotFound();
            }
            ViewData["PanierID"] = new SelectList(_context.Panier, "PanierID", "PanierID", lignePanier.PanierID);
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", lignePanier.ProduitID);
            return View(lignePanier);
        }

        // POST: LignePaniers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LignePanierID,PanierID,ProduitID,Qte")] LignePanier lignePanier)
        {
            if (id != lignePanier.LignePanierID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lignePanier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LignePanierExists(lignePanier.LignePanierID))
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
            ViewData["PanierID"] = new SelectList(_context.Panier, "PanierID", "PanierID", lignePanier.PanierID);
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", lignePanier.ProduitID);
            return View(lignePanier);
        }

        // GET: LignePaniers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LignePanier == null)
            {
                return NotFound();
            }

            var lignePanier = await _context.LignePanier
                .Include(l => l.panier)
                .Include(l => l.produit)
                .FirstOrDefaultAsync(m => m.LignePanierID == id);
            if (lignePanier == null)
            {
                return NotFound();
            }

            return View(lignePanier);
        }

        // POST: LignePaniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LignePanier == null)
            {
                return Problem("Entity set 'mvc_gogContext.LignePanier'  is null.");
            }
            var lignePanier = await _context.LignePanier.FindAsync(id);
            if (lignePanier != null)
            {
                _context.LignePanier.Remove(lignePanier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LignePanierExists(int id)
        {
          return (_context.LignePanier?.Any(e => e.LignePanierID == id)).GetValueOrDefault();
        }
    }
}
