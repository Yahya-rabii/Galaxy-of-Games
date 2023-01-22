namespace mvc_gog.Models
{
    public class CartViewModel
    {
        public IEnumerable<Panier> Paniers { get; set; }
        public IEnumerable<LignePanier> LignePaniers { get; set; }
    }


}
