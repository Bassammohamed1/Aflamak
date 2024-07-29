namespace Aflamak.Models.ViewModels
{
    public class SearchVM
    {
        public List<Film> Films { get; set; }
        public List<TvShow> TvShows { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Producer> Producers { get; set; }
    }
}
