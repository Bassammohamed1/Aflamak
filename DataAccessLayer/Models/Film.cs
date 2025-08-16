using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsSeries { get; set; }
        public int? PartsNo { get; set; }
        public int? Part { get; set; }
        public string? Root { get; set; }
        public int NoOfLikes { get; set; }
        public int NoOfDisLikes { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Required]
        public int? Type { get; set; } 
        [Required]
        public int? Language { get; set; } 
        [NotMapped]
        public IFormFile clientFile { get; set; }
        public byte[]? dbImage { get; set; }
        public int ProducerId { get; set; }
        [ForeignKey(nameof(ProducerId))]
        public Producer? Producer { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
        public List<ActorFilms>? ActorFilms { get; set; }
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
