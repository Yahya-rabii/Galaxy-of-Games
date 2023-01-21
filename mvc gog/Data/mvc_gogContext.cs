using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mvc_gog.Models;

namespace mvc_gog.Data
{
    public class mvc_gogContext : DbContext
    {
        
        public mvc_gogContext (DbContextOptions<mvc_gogContext> options)
            : base(options)
        {
        }

        public DbSet<mvc_gog.Models.Produit> Produit { get; set; } = default!;

        public DbSet<mvc_gog.Models.Panier> Panier { get; set; } = default!;

        public DbSet<mvc_gog.Models.LignePanier> LignePanier { get; set; } = default!;

        public DbSet<mvc_gog.Models.User> User { get; set; } = default!;
    }
}
