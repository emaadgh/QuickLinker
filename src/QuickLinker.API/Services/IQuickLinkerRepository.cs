using QuickLinker.API.Entities;

namespace QuickLinker.API.Services
{
    public interface IQuickLinkerRepository
    {
        Task<IEnumerable<ShortenedURL>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShortenedURL?> GetOriginalURLAsync(string shortCode, CancellationToken cancellationToken);
        Task AddShortenedURL(ShortenedURL shortenedURL, CancellationToken cancellationToken);
        void DeleteShortenedURL(ShortenedURL shortenedURL);
        Task<bool> SaveAsync(CancellationToken cancellationToken);
    }
}
