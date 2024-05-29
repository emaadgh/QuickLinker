using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using QuickLinker.API.Controllers;
using QuickLinker.API.Entities;
using QuickLinker.API.Models;
using QuickLinker.API.Services;
using System.Text;

namespace QuickLinker.Test.Controllers
{
    public class ShortLinkControllerTests
    {
        private readonly Mock<IQuickLinkerRepository> _mockRepository;
        private readonly Mock<IShortLinkService> _mockShortLinkService;
        private readonly Mock<ProblemDetailsFactory> _mockProblemDetailsFactory;
        private readonly Mock<IDistributedCache> _mockDistributedCache;
        private readonly ShortLinkController _controller;

        private readonly string domainURL = "https://localhost:7132/";
        private readonly string shortCode = "rGu2aeQORK";
        private readonly string originalURL = "https://www.google.com";

        public ShortLinkControllerTests()
        {
            _mockRepository = new Mock<IQuickLinkerRepository>();
            _mockShortLinkService = new Mock<IShortLinkService>();
            _mockProblemDetailsFactory = new Mock<ProblemDetailsFactory>();
            _mockDistributedCache = new Mock<IDistributedCache>();

            var inMemorySettings = new Dictionary<string, string?> {
                {"QuickLinkerDomain:Domain", domainURL}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new ShortLinkController(
                _mockRepository.Object,
                _mockShortLinkService.Object,
                configuration,
                _mockProblemDetailsFactory.Object,
                _mockDistributedCache.Object
            );
        }

        [Fact]
        public async Task CreateShortLink_ValidInput_MustReturnsShortenedUrl()
        {
            // Arrange
            var shortenedURLForCreationDTO = new ShortenedURLForCreationDTO { OriginalURL = originalURL };
            _mockShortLinkService.Setup(x => x.GenerateShortLink(shortenedURLForCreationDTO.OriginalURL)).Returns(shortCode);

            // Act
            var result = await _controller.CreateShortLink(shortenedURLForCreationDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var shortenedUrlToReturn = Assert.IsType<string>(okResult.Value);
            Assert.Equal(domainURL + shortCode, shortenedUrlToReturn);
        }

        [Fact]
        public async Task GetOriginalLink_ValidShortLink_MustReturnsRedirectResult()
        {
            // Arrange
            var shortLinkURL = domainURL + shortCode;

            var shortenedURL = new ShortenedURL(shortCode, originalURL);

            _mockRepository.Setup(x => x.GetOriginalURLAsync(shortLinkURL)).ReturnsAsync(shortenedURL);

            _mockDistributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns((string key, CancellationToken token) =>
              {
                  return Task.FromResult<byte[]?>(null);
              });

            // Act
            var result = await _controller.GetOriginalLink(shortLinkURL);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(originalURL, redirectResult.Url);
        }

        [Fact]
        public async Task GetOriginalLink_ValidShortLinkAndItsCached_MustReturnsRedirectResult()
        {
            // Arrange
            var shortLinkURL = domainURL + shortCode;

            var shortenedURL = new ShortenedURL(shortCode, originalURL);

            _mockRepository.Setup(x => x.GetOriginalURLAsync(shortLinkURL)).ReturnsAsync(shortenedURL);

            _mockDistributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns((string key, CancellationToken token) =>
              {
                  return Task.FromResult<byte[]?>(Encoding.UTF8.GetBytes(originalURL));
              });

            // Act
            var result = await _controller.GetOriginalLink(shortLinkURL);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(originalURL, redirectResult.Url);
        }

        [Fact]
        public async Task GetOriginalLink_InvalidShortLink_MustReturnsNotFoundResult()
        {
            // Arrange
            var shortlinkURL = "invalidShortLink";
            _mockRepository.Setup(x => x.GetOriginalURLAsync(shortlinkURL)).ReturnsAsync((ShortenedURL?)null);

            _mockDistributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns((string key, CancellationToken token) =>
              {
                  return Task.FromResult<byte[]?>(null);
              });

            // Act
            var result = await _controller.GetOriginalLink(shortlinkURL);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOriginalLink_NullShortLink_MustReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.GetOriginalLink(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOriginalLink_EmptyShortLink_MustReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.GetOriginalLink("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
