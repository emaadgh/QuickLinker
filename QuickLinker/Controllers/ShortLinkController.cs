using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuickLinker.API.Entities;
using QuickLinker.API.Models;
using QuickLinker.API.Services;

namespace QuickLinker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinkController : ControllerBase
    {
        private readonly IQuickLinkerRepository _quickLinkerRepository;
        private readonly IShortLinkService _shortLinkService;
        private readonly IConfiguration _configuration;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        private readonly string? domainURL;

        public ShortLinkController(IQuickLinkerRepository quickLinkerRepository, IShortLinkService shortLinkService,
            IConfiguration configuration, ProblemDetailsFactory problemDetailsFactory)
        {
            _quickLinkerRepository = quickLinkerRepository;
            _shortLinkService = shortLinkService;
            _configuration = configuration;
            _problemDetailsFactory = problemDetailsFactory;

            if (!string.IsNullOrEmpty(_configuration["QuickLinkerDomain:Domain"]))
            {
                domainURL = _configuration["QuickLinkerDomain:Domain"];
            }
            else
            {
                throw new Exception("QuickLinkerDomain:Domain configuration value is not found. Please define it in the configuration before running the app!");
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateShortLink(ShortenedURLForCreationDTO shortenedURLForCreationDTO)
        {
            var shortUrl = _shortLinkService.GenerateShortLink(shortenedURLForCreationDTO.OriginalURL);

            ShortenedURL shortenedURL = new ShortenedURL(shortUrl, shortenedURLForCreationDTO.OriginalURL);

            await _quickLinkerRepository.AddShortenedURL(shortenedURL);
            await _quickLinkerRepository.SaveAsync();

            var shortenedUrlToReturn = domainURL + shortenedURL.ShortCode;

            return Ok(shortenedUrlToReturn);
        }

        [HttpGet]
        [Route("/{shortlinkURL}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOriginalLink([FromRoute] string shortlinkURL)
        {
            if (string.IsNullOrEmpty(shortlinkURL))
            {
                return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"url cannot be null or empty, Enter a valid shortened URL"));
            }

            ShortenedURL? shortenedURL = await _quickLinkerRepository.GetOriginalURLAsync(shortlinkURL);

            if (shortenedURL == null)
            {
                return NotFound();
            }
            else
            {
                var originalURLToReturn = shortenedURL.OriginalURL;

                return Redirect(originalURLToReturn);
            }
        }
    }
}
