using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        public List<Film>? Films { get; set; }
        public List<TvShow>? TvShows { get; set; }
    }
}
