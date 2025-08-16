using DataAccessLayer.Models;

namespace DataAccessLayer.Models.ViewModels
{
    public class EpisodeVM
    {
        public List<Episode> Episodes { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
