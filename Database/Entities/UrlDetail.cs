using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class UrlDetail
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
