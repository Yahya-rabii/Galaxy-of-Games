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
    public class PaniersController : Controller
    {
        private readonly mvc_gogContext _context;

        public PaniersController(mvc_gogContext context)
        {
            _context = context;
        }

        // GET: Paniers
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var registeredUser = _context.User.FirstOrDefault(u => u.UserID == userId);

            if (registeredUser.IsAdmin)
            {
                // return all paniers
                var paniers= await _context.Panier.Include(p=>p.User).Include(p=>p.LignePanier).ThenInclude(lp=>lp.produit).ToListAsync();
                var lignepaniers = await _context.LignePanier.Include(lp=>lp.produit).ToListAsync();
                

                var model = new CartViewModel
                {
                    Paniers = paniers,
                    LignePaniers = lignepaniers
                };
                return View(model);


            }

            else
            {
                // return only the user's paniers
                var panier = await _context.Panier.Include(p => p.User).Include(p => p.LignePanier).ThenInclude(lp => lp.produit).FirstOrDefaultAsync(p=>p.User.UserID == userId);
               
                var model = new CartViewModel
                {
                    Paniers = new List<Panier> { panier ?? new Panier()},
                    LignePaniers =panier?.LignePanier ??new List<LignePanier>()
                };
                
                
                return View(model);
            }



        }



        // GET: Paniers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Panier == null)
            {
                return NotFound();
            }
            var lignepanier = await _context.LignePanier.FindAsync(id);
            return RedirectToAction("Details", "LignePaniers", new { id = lignepanier.LignePanierID });
        }


        // GET: Paniers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Panier == null)
            {
                return NotFound();
            }

            var lignepanier = await _context.LignePanier.FindAsync(id);
            if (lignepanier == null)
            {
                return NotFound();
            }

            return RedirectToAction("Edit", "LignePaniers", new { id = lignepanier.LignePanierID });
        }




        // GET: Paniers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Panier == null)
            {
                return NotFound();
            }

            var lignepanier = await _context.LignePanier.FindAsync(id);
            if (lignepanier == null)
            {
              
                return NotFound();
            }

            return RedirectToAction("Delete", "LignePaniers", new { id = lignepanier.LignePanierID });
        }

        // POST: Paniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            

            if (_context.Panier == null)
            {
                return Problem("Entity set 'mvc_gogContext.Panier'  is null.");
            }
            var lignepanier = await _context.LignePanier.FindAsync(id);

            if (lignepanier == null)
            {
               
                return NotFound();
            }

            return RedirectToAction("DeleteConfirmed", "LignePaniers", new { id = lignepanier.LignePanierID });
        }

        private bool PanierExists(int id)
        {
          return (_context.Panier?.Any(e => e.PanierID == id)).GetValueOrDefault();
        }
    }
}
