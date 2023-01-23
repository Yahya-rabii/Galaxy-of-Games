using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_gog.Data;
using mvc_gog.Models;

namespace mvc_gog.Controllers
{
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
            var mvc_gogContext = _context.LignePanier.Include(l => l.produit);
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
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID");
            return View();
        }

        // POST: LignePaniers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LignePanierID,PanierID,ProduitID,NbreArticle,Total")] LignePanier lignePanier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lignePanier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["ProduitID"] = new SelectList(_context.Produit, "ProduitID", "ProduitID", lignePanier.ProduitID);
            return View(lignePanier);
        }

        // POST: LignePaniers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LignePanierID,PanierID,ProduitID,NbreArticle,Total")] LignePanier lignePanier)
        {
            if (id != lignePanier.LignePanierID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {


                    var existingLignePanier = _context.LignePanier.Include(lp => lp.produit).FirstOrDefault(lp => lp.LignePanierID == id);


                    if (existingLignePanier is null) return NotFound();


                    existingLignePanier.NbreArticle = lignePanier.NbreArticle;
                    existingLignePanier.Total = lignePanier.NbreArticle * existingLignePanier.produit.Price;




                    Panier panier = _context.Panier.Include(p => p.LignePanier).FirstOrDefault(lp => lp.PanierID == existingLignePanier.PanierID);



                    panier.NbreArticle = panier.LignePanier.Sum(p => p.NbreArticle);
                    panier.Total = panier.LignePanier.Sum(p => p.Total);





                    _context.Update(existingLignePanier);
                    _context.Update(panier);

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
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Paniers", new { id = lignePanier.LignePanierID });

            }
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





                /////////////////////////////////////////////////
                _context.LignePanier.Remove(lignePanier);
                await _context.SaveChangesAsync();

                Panier panier = _context.Panier.Include(p => p.LignePanier).FirstOrDefault(lp => lp.PanierID == lignePanier.PanierID);

                panier.NbreArticle = panier.LignePanier.Sum(p => p.NbreArticle);
                panier.Total = panier.LignePanier.Sum(p => p.Total);

                _context.Update(panier);
                await _context.SaveChangesAsync();


                if (_context.LignePanier == null)
                {
                    _context.Panier.RemoveRange(_context.Panier);
                    await _context.SaveChangesAsync();
                }

                /////////////////////////////////////////////////

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Paniers", new { id = lignePanier.LignePanierID });

        }

        private bool LignePanierExists(int id)
        {
            return (_context.LignePanier?.Any(e => e.LignePanierID == id)).GetValueOrDefault();
        }
    }
}
