namespace QuickLinker.API.Services
{
    public interface IShortLinkService
    {
        public string GenerateShortLink(string originalURL);
    }
}
