namespace mvc_gog.Models
{
    public class LignePanier
    {

        public int LignePanierID { get; set; }
        public int PanierID { get; set; }
        public int ProduitID { get; set; }
        public int Qte { get; set; }

        public Produit? produit { get; set; }
        public Panier? panier { get; set; }
    }
}
