
namespace mvc_gog.Models
{
    public class Panier
    {
        public int PanierID { get; set; }

        public int NbreArticle { get; set; }

        public string? Prdname { get; set; }
        public int Produit { get; set; }

        public double Total { get; set; }
             
        public ICollection<LignePanier>? LignePanier { get; set; }

    }
}
