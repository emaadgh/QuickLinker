using QuickLinker.API.Entities;

namespace QuickLinker.API.Services
{
    public interface IQuickLinkerRepository
    {
        Task<IEnumerable<ShortenedURL>> GetAllAsync();
        Task<ShortenedURL?> GetOriginalURLAsync(string shortCode);
        Task AddShortenedURL(ShortenedURL shortenedURL);
        void DeleteShortenedURL(ShortenedURL shortenedURL);
        Task<bool> SaveAsync();
    }
}
