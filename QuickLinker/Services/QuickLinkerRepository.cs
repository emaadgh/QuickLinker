using Microsoft.EntityFrameworkCore;
using QuickLinker.API.DbContexts;
using QuickLinker.API.Entities;

namespace QuickLinker.API.Services
{
    public class QuickLinkerRepository : IQuickLinkerRepository
    {
        private readonly QuickLinkerDbContext _dbContext;

        public QuickLinkerRepository(QuickLinkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddShortenedURL(ShortenedURL shortenedURL)
        {
            if (shortenedURL == null)
            {
                throw new ArgumentNullException(nameof(shortenedURL));
            }

            if (await _dbContext.ShortenedURLs.Where(s => s.ShortCode.Equals(shortenedURL.ShortCode)).FirstOrDefaultAsync() == null)
            {
                _dbContext.ShortenedURLs.Add(shortenedURL);
            }
        }

        public void DeleteShortenedURL(ShortenedURL shortenedURL)
        {
            _dbContext.ShortenedURLs.Remove(shortenedURL);
        }

        public async Task<IEnumerable<ShortenedURL>> GetAllAsync()
        {
            return await _dbContext.ShortenedURLs.ToListAsync();
        }

        public async Task<ShortenedURL?> GetOriginalURLAsync(string shortCode)
        {
            return await _dbContext.ShortenedURLs.Where(s => s.ShortCode.Equals(shortCode)).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _dbContext.SaveChangesAsync() > 0);
        }
    }
}
