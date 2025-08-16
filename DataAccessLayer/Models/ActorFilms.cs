namespace DataAccessLayer.Models
{
    public class ActorFilms
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
    }
}
