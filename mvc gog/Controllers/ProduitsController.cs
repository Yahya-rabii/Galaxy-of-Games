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
    public class ProduitsController : Controller
    {
        private readonly mvc_gogContext _context;

        public ProduitsController(mvc_gogContext context)
        {
            _context = context;
        }

        // GET: Produits
        public async Task<IActionResult> Index()
        {
              return _context.Produit != null ? 
                          View(await _context.Produit.ToListAsync()) :
                          Problem("Entity set 'mvc_gogContext.Produit'  is null.");
        }

        // GET: Produits but for clients

        public async Task<IActionResult> List()
        {
            return _context.Produit != null ?
                        View(await _context.Produit.ToListAsync()) :
                        Problem("Entity set 'mvc_gogContext.Produit'  is null.");
        }



        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduitID,imageurl,Prodname,Price,Dateofcreation,Datedeproduction,desc,Autor")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProduitID,imageurl,Prodname,Price,Dateofcreation,Datedeproduction,desc,Autor")] Produit produit)
        {
            if (id != produit.ProduitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.ProduitID))
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
            return View(produit);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produit == null)
            {
                return NotFound();
            }

            var produit = await _context.Produit
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produit == null)
            {
                return Problem("Entity set 'mvc_gogContext.Produit'  is null.");
            }
            var produit = await _context.Produit.FindAsync(id);
            if (produit != null)
            {
                _context.Produit.Remove(produit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
          return (_context.Produit?.Any(e => e.ProduitID == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> AddPanier(int id, int Quantite)
        {

        

            var produit = await _context.Produit.FindAsync(id);
            var panier = new Panier();

            if (produit != null && Quantite > 0)
            {

               
                    
                panier.NbreArticle = Quantite;

                panier.Total = Quantite * (float)produit.Price;

                panier.Produit = id;

                panier.Prdname = produit.Prodname;

                _context.Panier.Update(panier);
                _context.SaveChanges(); 
                
                

                

                    foreach (var tmp in _context.Panier)
                    {

                        if (tmp.Produit == id && tmp != panier)
                        {
                            panier.NbreArticle = panier.NbreArticle + tmp.NbreArticle;
                            panier.Total = panier.Total + tmp.Total;
                            panier.Prdname= tmp.Prdname;
                            _context.Panier.Remove(tmp);
                            _context.Panier.Update(panier);

                        }

                    }
                    _context.SaveChanges();

             



            }
            return RedirectToAction("Index", "Paniers");
        }

    }
}