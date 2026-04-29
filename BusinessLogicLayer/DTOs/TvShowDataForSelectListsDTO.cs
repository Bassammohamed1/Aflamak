using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class TvShowDataForSelectListsDTO
    {
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
