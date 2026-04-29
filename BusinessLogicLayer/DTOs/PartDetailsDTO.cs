using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class PartDetailsDTO
    {
        public Part Part { get; set; }
        public List<Part> Parts { get; set; }
        public List<Episode> Episodes { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
