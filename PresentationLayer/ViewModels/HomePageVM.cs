using DataAccessLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class HomePageVM
    {
        public IEnumerable<Film> MostWatchedFilms { get; set; }
        public IEnumerable<TvShow> MostWatchedTvShows { get; set; }
        public IEnumerable<Episode> RecentEpisodes { get; set; }
        public IEnumerable<Film> RecentFilms { get; set; }
        public IEnumerable<TvShow> RecentTvShows { get; set; }
        public IEnumerable<Film> ArabicFilms { get; set; }
        public IEnumerable<TvShow> ArabicTvShows { get; set; }
        public IEnumerable<Film> CartoonFilms { get; set; }
        public IEnumerable<TvShow> RamadanTvShows { get; set; }
    }
}
