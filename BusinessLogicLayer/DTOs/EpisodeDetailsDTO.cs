using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class EpisodeDetailsDTO
    {
        public Episode Episode { get; set; }
        public List<Episode> Episodes { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
