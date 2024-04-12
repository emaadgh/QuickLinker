using System.ComponentModel.DataAnnotations;

namespace QuickLinker.API.Models
{
    public class ShortenedURLForCreationDTO
    {
        [Required(ErrorMessage = "You should fill out a URL.")]
        [MaxLength(1000, ErrorMessage = "The URL shouldn't have more than 1000 characters.")]
        public string OriginalURL { get; set; } = string.Empty;
    }
}
