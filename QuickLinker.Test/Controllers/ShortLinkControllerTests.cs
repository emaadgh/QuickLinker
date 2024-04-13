using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using QuickLinker.API.Controllers;
using QuickLinker.API.Entities;
using QuickLinker.API.Models;
using QuickLinker.API.Services;

namespace QuickLinker.Test.Controllers
{
    public class ShortLinkControllerTests
    {
        private readonly Mock<IQuickLinkerRepository> _mockRepository;
        private readonly Mock<IShortLinkService> _mockShortLinkService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ProblemDetailsFactory> _mockProblemDetailsFactory;
        private readonly ShortLinkController _controller;

        private readonly string domainURL = "https://localhost:7132/";
        private readonly string shortCode = "rGu2aeQORK";
        private readonly string originalURL = "https://www.google.com";

        public ShortLinkControllerTests()
        {
            _mockRepository = new Mock<IQuickLinkerRepository>();
            _mockShortLinkService = new Mock<IShortLinkService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockProblemDetailsFactory = new Mock<ProblemDetailsFactory>();

            _mockConfiguration.Setup(x => x["QuickLinkerDomain:Domain"]).Returns(domainURL);

            _controller = new ShortLinkController(
                _mockRepository.Object,
                _mockShortLinkService.Object,
                _mockConfiguration.Object,
                _mockProblemDetailsFactory.Object
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
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOriginalLink_EmptyShortLink_MustReturnsBadRequestResult()
        {
            // Act
            var result = await _controller.GetOriginalLink("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
