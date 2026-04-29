using DataAccessLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class EpisodeDetailsVM
    {
        public Episode Episode { get; set; }
        public List<Episode> Episodes { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
