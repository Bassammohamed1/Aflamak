namespace DataAccessLayer.Models.ViewModels
{
    public class FilmVM
    {
        public List<Film> Films { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool FromHome { get; set; }
        public bool Arabic { get; set; }
        public bool Cartoon { get; set; }
    }
}
