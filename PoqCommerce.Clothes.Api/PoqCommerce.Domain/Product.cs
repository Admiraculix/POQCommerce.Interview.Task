namespace PoqCommerce.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public List<string> Sizes { get; set; }
        public string Description { get; set; }
    }
}