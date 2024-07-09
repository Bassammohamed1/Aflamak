namespace Aflamak.Models.ViewModels
{
    public class TvShowDetailsVM
    {
        public TvShow TvShow { get; set; }
        public List<TvShow> RelatedTvShows { get; set; }
        public List<Part> TvShowParts { get; set; }
        public bool HasUserLiked { get; set; }
        public bool HasUserDisliked { get; set; }
    }
}
