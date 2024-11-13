using QuickLinker.API.Entities;

namespace QuickLinker.API.Services
{
    public interface IQuickLinkerRepository
    {
        Task<IEnumerable<ShortenedURL>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ShortenedURL?> GetOriginalURLAsync(string shortCode, CancellationToken cancellationToken = default);
        Task AddShortenedURL(ShortenedURL shortenedURL, CancellationToken cancellationToken = default);
        void DeleteShortenedURL(ShortenedURL shortenedURL);
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);
    }
}
