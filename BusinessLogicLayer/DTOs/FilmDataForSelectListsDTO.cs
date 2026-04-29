using DataAccessLayer.Models;

namespace BusinessLogicLayer.DTOs
{
    public class FilmDataForSelectListsDTO
    {
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
