﻿using Microsoft.EntityFrameworkCore;
using QuickLinker.API.DbContexts;
using QuickLinker.API.Entities;
using System.Threading;

namespace QuickLinker.API.Services
{
    public class QuickLinkerRepository : IQuickLinkerRepository
    {
        private readonly QuickLinkerDbContext _dbContext;

        public QuickLinkerRepository(QuickLinkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddShortenedURL(ShortenedURL shortenedURL, CancellationToken cancellationToken = default)
        {
            if (shortenedURL == null)
            {
                throw new ArgumentNullException(nameof(shortenedURL));
            }

            var shortenedFromDatabase = await _dbContext.ShortenedURLs.FirstOrDefaultAsync(
                                                                       s => s.ShortCode.Equals(shortenedURL.ShortCode),
                                                                       cancellationToken);

            if (shortenedFromDatabase == null)
            {
                _dbContext.ShortenedURLs.Add(shortenedURL);
            }
        }

        public void DeleteShortenedURL(ShortenedURL shortenedURL)
        {
            _dbContext.ShortenedURLs.Remove(shortenedURL);
        }

        public async Task<IEnumerable<ShortenedURL>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.ShortenedURLs.ToListAsync(cancellationToken);
        }

        public async Task<ShortenedURL?> GetOriginalURLAsync(string shortCode, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ShortenedURLs.FirstOrDefaultAsync(s => s.ShortCode.Equals(shortCode), cancellationToken);
        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            return (await _dbContext.SaveChangesAsync(cancellationToken) > 0);
        }
    }
}
