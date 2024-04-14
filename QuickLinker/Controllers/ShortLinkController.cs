using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _distributedCache;

        private readonly string? domainURL;
        private readonly int cacheExpirationInDays = 15;

        public ShortLinkController(IQuickLinkerRepository quickLinkerRepository, IShortLinkService shortLinkService,
            IConfiguration configuration, ProblemDetailsFactory problemDetailsFactory, IDistributedCache distributedCache)
        {
            _quickLinkerRepository = quickLinkerRepository;
            _shortLinkService = shortLinkService;
            _configuration = configuration;
            _problemDetailsFactory = problemDetailsFactory;
            _distributedCache = distributedCache;

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

            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(cacheExpirationInDays)
            };

            await _distributedCache.SetStringAsync(shortenedURL.ShortCode, shortenedURL.OriginalURL, options);

            var shortenedUrlToReturn = domainURL + shortenedURL.ShortCode;

            return Ok(shortenedUrlToReturn);
        }

        [HttpGet]
        [Route("/{shortCode}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOriginalLink([FromRoute] string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"url cannot be null or empty, Enter a valid shortened URL"));
            }

            var originalURLToReturn = await _distributedCache.GetStringAsync(shortCode);

            if (originalURLToReturn == null)
            {
                ShortenedURL? shortenedURL = await _quickLinkerRepository.GetOriginalURLAsync(shortCode);

                if (shortenedURL == null)
                {
                    return NotFound();
                }
                else
                {
                    DistributedCacheEntryOptions options = new()
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(cacheExpirationInDays)
                    };

                    await _distributedCache.SetStringAsync(shortenedURL.ShortCode, shortenedURL.OriginalURL, options);

                    originalURLToReturn = shortenedURL.OriginalURL;

                    return Redirect(originalURLToReturn);
                }
            }

            return Redirect(originalURLToReturn);
        }
    }
}
