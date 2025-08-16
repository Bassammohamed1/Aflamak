using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Models
{
    public class Part
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("عدد الحلقات")]
        public int EpisodesNo { get; set; }
        [DisplayName("عدد الأعجابات")]
        public int NoOfLikes { get; set; }
        [DisplayName("عدد عدم الأعجاب")]
        public int NoOfDisLikes { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("السنة")]
        public int Date { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الشهر")]
        public int Month { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("المسلسل")]
        public int TvShowId { get; set; }
        [ForeignKey(nameof(TvShowId))]
        public TvShow? TvShow { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الصورة")]
        [NotMapped]
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
