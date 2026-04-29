using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class EpisodeDTO
    {
        public List<Episode> Episodes { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
