namespace Aflamak.Models.ViewModels
{
    public class FilmDetailsVM
    {
        public Film Film { get; set; }
        public List<Film> RelatedFilms { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}