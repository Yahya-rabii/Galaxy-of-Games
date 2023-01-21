namespace mvc_gog.Models
{
    public class Produit
    {

        public int ProduitID { get; set; }

        public string? imageurl { get; set; }
        public string? Prodname { get; set; }
        public float Price { get; set; }
        public DateTime Dateofcreation { get; set; }
        public DateTime Datedeproduction { get; set; }

        public string? desc { get; set; }

        public string? Autor { get; set; }

        public ICollection<LignePanier>? LignePanier { get; set; }

        public static implicit operator int(Produit? v)
        {
            throw new NotImplementedException();
        }
    }
}
