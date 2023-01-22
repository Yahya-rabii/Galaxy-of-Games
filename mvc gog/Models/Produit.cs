namespace mvc_gog.Models
{
    public class Produit
    {

        public int ProduitID { get; set; }
        public int PanierID { get; set; }

        public string? imageurl { get; set; }
        public string? Prodname { get; set; }
        public float Price { get; set; }
        public DateTime Dateofcreation { get; set; } = DateTime.Now;

        public string? desc { get; set; }

        public string? Autor { get; set; }

      
    }
}
