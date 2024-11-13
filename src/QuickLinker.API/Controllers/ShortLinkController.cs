using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using QuickLinker.API.Entities;
using QuickLinker.API.Models;
using QuickLinker.API.Observability;
using QuickLinker.API.Services;

namespace QuickLinker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinkController : ControllerBase
    {
        private readonly IQuickLinkerRepository _quickLinkerRepository;
        private readonly IShortLinkService _shortLinkService;
        private readonly ProblemDetailsFactory _problemDetailsFactory;
        private readonly IDistributedCache _distributedCache;
        private readonly IQuickLinkerDiagnostic _quickLinkerDiagnostic;

        private readonly string? domainURL;
        private readonly int cacheExpirationInDays = 15;

        /// <summary>
        /// Constructor for ShortLinkController.
        /// </summary>
        public ShortLinkController(
            IQuickLinkerRepository quickLinkerRepository,
            IShortLinkService shortLinkService,
            IConfiguration configuration,
            ProblemDetailsFactory problemDetailsFactory,
            IDistributedCache distributedCache,
            IQuickLinkerDiagnostic quickLinkerDiagnostic)
        {
            _quickLinkerRepository = quickLinkerRepository;
            _shortLinkService = shortLinkService;
            _problemDetailsFactory = problemDetailsFactory;
            _distributedCache = distributedCache;
            _quickLinkerDiagnostic = quickLinkerDiagnostic;

            AppSettings? appSettings = configuration.Get<AppSettings>();

            if (appSettings is not null && appSettings.QuickLinkerDomain.Domain is not null)
            {
                domainURL = appSettings.QuickLinkerDomain.Domain;
            }
            else
            {
                throw new Exception("QuickLinkerDomain:Domain configuration value is not found. Please define it in the configuration before running the app!");
            }
        }

        /// <summary>
        /// Creates a short link for the provided original URL.
        /// </summary>
        /// <param name="shortenedURLForCreationDTO">DTO containing the original URL.</param>
        /// <param name="cancellationToken"> Cancellation token</param>
        /// <returns>The shortened URL.</returns>
        [HttpPost(Name = "CreateShortLink")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateShortLink(
            ShortenedURLForCreationDTO shortenedURLForCreationDTO,
            CancellationToken cancellationToken)
        {
            var shortUrl = _shortLinkService.GenerateShortLink(shortenedURLForCreationDTO.OriginalURL);

            ShortenedURL shortenedURL = new ShortenedURL(shortUrl, shortenedURLForCreationDTO.OriginalURL);

            await _quickLinkerRepository.AddShortenedURL(shortenedURL, cancellationToken);
            await _quickLinkerRepository.SaveAsync(cancellationToken);

            var shortenedUrlToReturn = domainURL + shortenedURL.ShortCode;

            _quickLinkerDiagnostic.AddShortLink();

            return Ok(shortenedUrlToReturn);
        }

        /// <summary>
        /// Redirects to the original URL associated with the provided short code.
        /// </summary>
        /// <param name="shortCode">Short code to look up the original URL.</param>
        /// <param name="cancellationToken"> Cancellation token</param>
        /// <returns>Redirect to the original URL.</returns>
        [HttpGet("/{shortCode}", Name = "GetOriginalLink")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOriginalLink(
            [FromRoute] string shortCode,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"url cannot be null or empty, Enter a valid shortened URL"));
            }

            _quickLinkerDiagnostic.AddOriginalLink();

            var originalURLToReturn = await _distributedCache.GetStringAsync(shortCode, cancellationToken);

            if (originalURLToReturn == null)
            {
                ShortenedURL? shortenedURL = await _quickLinkerRepository.GetOriginalURLAsync(shortCode, cancellationToken);

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

                    await _distributedCache.SetStringAsync(shortenedURL.ShortCode, shortenedURL.OriginalURL, options, cancellationToken);

                    originalURLToReturn = shortenedURL.OriginalURL;

                    return Redirect(originalURLToReturn);
                }
            }

            return Redirect(originalURLToReturn);
        }
    }
}
