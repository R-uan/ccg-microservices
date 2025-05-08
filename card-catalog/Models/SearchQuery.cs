namespace CardCatalog.Models
{
    public class SearchQuery
    {
        public List<int>? Rarity { get; set; }
        public List<int?>? Attack { get; set; }
        public List<int?>? Health { get; set; }
        public List<int>? ManaCost { get; set; }
        public List<string>? Categories { get; set; }
    }
}