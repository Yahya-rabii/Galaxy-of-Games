using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Create([Bind("ProduitID,PanierID,imageurl,Prodname,Price,Dateofcreation,desc,Autor")] Produit produit)
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
        public async Task<IActionResult> Edit(int id, [Bind("ProduitID,PanierID,imageurl,Prodname,Price,Dateofcreation,desc,Autor")] Produit produit)
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




        public async Task<IActionResult> AddToPanier(int id, int Quantite)
        {
            //GET THE LOGGED IN USER ID 
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // GET THE PRODUCT FROM DB
            var produit = await _context.Produit.FindAsync(id);

            // check if product exists and the quantity is valid
            if (produit == null || Quantite <= 0)
            {
                return RedirectToAction("Index", "Paniers");
            }

            // check if user already has a PANIER
            var panier = await _context.Panier.Include(p => p.LignePanier).FirstOrDefaultAsync(p => p.User.UserID == userId);

            if (panier == null)
            {
                panier = new Panier();
                var usr =  _context.User.FirstOrDefaultAsync(p => p.UserID == userId);
                panier.User = usr.Result;

                panier.NbreArticle += Quantite;

                panier.Total += Quantite * (double)produit.Price;

                //create new line item for the PANIER
                var lignePanier = new LignePanier { ProduitID = produit.ProduitID, NbreArticle = Quantite, Total = Quantite * produit.Price, produit=produit};
                panier.LignePanier = new List<LignePanier> { lignePanier };
               
                _context.Panier.Add(panier);
                await _context.SaveChangesAsync();
            }
            else
            {
                // check if product is already in the PANIER
                var existingLignePanier = panier.LignePanier.FirstOrDefault(lp => lp.ProduitID == id);

                if (existingLignePanier != null)
                {
                    // update the panier and lignepanier
                    existingLignePanier.NbreArticle += Quantite;
                    existingLignePanier.Total += Quantite * (double)produit.Price;

                    panier.NbreArticle += Quantite;
                    panier.Total += Quantite * (double)produit.Price;
                    
                    _context.Update(panier);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // add new product to the lignepanier
                    var lignePanier = new LignePanier { ProduitID = produit.ProduitID, NbreArticle = Quantite, Total = Quantite * produit.Price, produit = produit };


                    panier.LignePanier.Add(lignePanier);

                    panier.NbreArticle += Quantite;
                    panier.Total += Quantite * (double)produit.Price;
                    _context.Update(panier);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index", "Paniers");
        }



    }
}
