using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickLinker.API.Entities
{
    [Index(nameof(ShortCode), IsUnique = true)]
    public class ShortenedURL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string ShortCode { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string OriginalURL { get; set; } = null!;

        [Required]
        public DateTimeOffset CreationDate { get; set; }

        public ShortenedURL(string shortCode, string originalURL)
        {
            ShortCode = shortCode;
            OriginalURL = originalURL;
            CreationDate = DateTimeOffset.UtcNow;
        }
    }
}
