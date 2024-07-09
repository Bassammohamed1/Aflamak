namespace Aflamak.Models
{
    public class ActorTvShows
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int TvShowId { get; set; }
        public TvShow TvShow { get; set; }
    }
}
