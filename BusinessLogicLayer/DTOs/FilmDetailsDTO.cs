using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class FilmDetailsDTO
    {
        public Film Film { get; set; }
        public List<Film> RelatedFilms { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
