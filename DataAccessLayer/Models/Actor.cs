using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(100)]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(100)]
        [DisplayName("الأسم الآخر")]
        public string AnotherLangName { get; set; }
        public List<ActorFilms>? ActorFilms { get; set; }
        public List<ActorTvShows>? ActorTvShows { get; set; }
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
