using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Aflamak.Models
{
    public class Producer
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(100)]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(100)]
        [DisplayName("الأسم الآخر")]
        public string AnotherLangName { get; set; }
        public List<Film>? Films { get; set; }
        public List<TvShow>? TvShows { get; set; }
        [NotMapped]
        [DisplayName("الصورة")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public IFormFile clientFile { get; set; }
        public byte[]? dbImage { get; set; }
        [NotMapped]
        public string? imageSrc
        {
            get
            {
                if (dbImage != null)
                {
                    string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
