namespace Aflamak.Models.ViewModels
{
    public class HomePageVM
    {
        public IQueryable<Film> MostWatchedFilms { get; set; }
        public IQueryable<TvShow> MostWatchedTvShows { get; set; }
        public IQueryable<Episode> RecentEpisodes { get; set; }
        public IQueryable<Film> RecentFilms { get; set; }
        public IQueryable<TvShow> RecentTvShows { get; set; }
        public IQueryable<Film> ArabicFilms { get; set; }
        public IQueryable<TvShow> ArabicTvShows { get; set; }
        public IQueryable<Film> CartoonFilms { get; set; }
        public IQueryable<TvShow> RamadanTvShows { get; set; }
    }
}
