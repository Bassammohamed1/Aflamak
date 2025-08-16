using DataAccessLayer.Models;

namespace DataAccessLayer.Models.ViewModels
{
    public class PartDetailsVM
    {
        public Part Part { get; set; }
        public List<Part> Parts { get; set; }
        public List<Episode> Episodes { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
