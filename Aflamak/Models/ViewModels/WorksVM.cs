namespace Aflamak.Models.ViewModels
{
    public class WorksVM
    {
        public List<ItemViewModel> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool FromHome { get; set; }
        public bool Film { get; set; }
        public bool TvShow { get; set; }
    }
}
