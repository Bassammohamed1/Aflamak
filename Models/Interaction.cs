namespace Aflamak.Models
{
    public class Interaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisLiked { get; set; }
    }
}
