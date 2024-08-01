namespace Aflamak.Models.ViewModels
{
    public class SearchVM
    {
        public IQueryable<Film> Films { get; set; }
        public IQueryable<TvShow> TvShows { get; set; }
        public IQueryable<Actor> Actors { get; set; }
        public IQueryable<Producer> Producers { get; set; }
    }
}
