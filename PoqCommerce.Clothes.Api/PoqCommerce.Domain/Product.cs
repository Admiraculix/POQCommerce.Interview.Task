namespace PoqCommerce.Domain
{
    public class Product
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public List<string> Sizes { get; set; }
        public string Description { get; set; }
    }
}