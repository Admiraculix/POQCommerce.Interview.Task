namespace PoqCommerce.Application.DTOs
{
    public class FilterObject
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> CommonWords { get; set; }
    }
}