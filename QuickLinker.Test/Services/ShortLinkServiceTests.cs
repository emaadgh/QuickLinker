using QuickLinker.API.Services;

namespace QuickLinker.Test.Services
{
    public class ShortLinkServiceTests
    {
        [Fact]
        public void GenerateShortLink_GeneratesExactShortCode_MustReturnsExactUniqueShortCodes()
        {
            // Arrange
            var service = new ShortLinkService();

            var url = "https://www.google.com";

            // Act
            var shortCode = service.GenerateShortLink(url);

            // Assert
            Assert.Equal("rGu2aeQORK", shortCode);
        }

        [Fact]
        public void GenerateShortLink_WithDifferentUrls_MustReturnsUniqueShortCodes()
        {
            // Arrange
            var service = new ShortLinkService();

            var url1 = "https://www.google.com/page1";
            var url2 = "https://www.google.com/page2";

            // Act
            var shortCode1 = service.GenerateShortLink(url1);
            var shortCode2 = service.GenerateShortLink(url2);

            // Assert
            Assert.NotEqual(shortCode1, shortCode2);
        }

        [Fact]
        public void GenerateShortLink_SameUrl_MustReturnsSameShortCode()
        {
            // Arrange
            var service = new ShortLinkService();
            var url = "https://www.google.com/page";

            // Act
            var shortCode1 = service.GenerateShortLink(url);
            var shortCode2 = service.GenerateShortLink(url);

            // Assert
            Assert.Equal(shortCode1, shortCode2);
        }

        [Fact]
        public void GenerateShortLink_EmptyUrl_MustReturnsEmptyString()
        {
            // Arrange
            var service = new ShortLinkService();
            var emptyUrl = string.Empty;

            // Act
            var shortCode = service.GenerateShortLink(emptyUrl);

            // Assert
            Assert.Equal(string.Empty, shortCode);
        }

    }
}
