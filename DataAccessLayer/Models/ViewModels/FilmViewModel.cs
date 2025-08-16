using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Models.ViewModels
{
    public class FilmViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب"), MaxLength(100)]
        [DisplayName("الأسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الوصف")]
        public string Description { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("له أجزاء")]
        public bool IsSeries { get; set; }
        [DisplayName("عدد الأجزاء")]
        public int? PartsNo { get; set; }
        [DisplayName("الجزء")]
        public int? Part { get; set; }
        [DisplayName("الأصل")]
        public string? Root { get; set; }
        [DisplayName("عدد الأعجابات")]
        public int NoOfLikes { get; set; }
        [DisplayName("عدد عدم الأعجاب")]
        public int NoOfDisLikes { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("السنة")]
        public int Year { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الشهر")]
        public int Month { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("فيلم / مسلسل")]
        public ItemType Type { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("اللغة")]
        public Languages Language { get; set; }
        [NotMapped]
        [DisplayName("الصورة")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public IFormFile clientFile { get; set; }
        public byte[]? dbImage { get; set; }
        [DisplayName("المخرج")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int ProducerId { get; set; }
        [DisplayName("الممثلين")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public List<int> ActorsId { get; set; }
        [DisplayName("النوع")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int CategoryId { get; set; }
        [DisplayName("البلد")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int CountryId { get; set; }
    }
}
