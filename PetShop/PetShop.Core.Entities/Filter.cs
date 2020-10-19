namespace PetShop.Core.Entities
{
    public class Filter
    {
        public int CurrentPage { get; set; }
        public int ItemsPrPage { get; set; }

        public string Name { get; set; }
        public string Sorting { get; set; }
        public string PetType { get; set; }
    }
}
