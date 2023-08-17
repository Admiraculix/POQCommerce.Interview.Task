namespace PoqCommerce.Application.Models
{
    public class FilterObject
    {
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string Size { get; set; }
        public string Highlight { get; set; }
    }
}
