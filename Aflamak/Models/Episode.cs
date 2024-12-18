using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aflamak.Models
{
    public class Episode
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الوصف")]
        public string Description { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("رقم الحلقة")]
        public int EpisodeNo { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("السنة")]
        public int Date { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DisplayName("الجزء")]
        public int PartId { get; set; }
        [ForeignKey(nameof(PartId))]
        [DisplayName("عدد الأعجابات")]
        public int NoOfLikes { get; set; }
        [DisplayName("عدد عدم الأعجاب")]
        public int NoOfDisLikes { get; set; }
        public Part? Part { get; set; }
    }
}