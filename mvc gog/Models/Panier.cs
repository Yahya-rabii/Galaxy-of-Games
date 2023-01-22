
namespace mvc_gog.Models
{
    public class Panier
    {
        public int PanierID { get; set; }

        public int NbreArticle { get; set; }

        public double Total { get; set; }
        public User? User { get; set; }

        public ICollection<LignePanier>? LignePanier { get; set; }

    }
}
