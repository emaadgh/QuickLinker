using System.ComponentModel.DataAnnotations;

namespace QuickLinker.API.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating a shortened URL.
    /// </summary>
    public class ShortenedURLForCreationDTO
    {
        /// <summary>
        /// The original URL to be shortened.
        /// </summary>
        [Required(ErrorMessage = "You should fill out a URL.")]
        [MaxLength(1000, ErrorMessage = "The URL shouldn't have more than 1000 characters.")]
        [Url]
        public string OriginalURL { get; set; } = string.Empty;
    }
}
