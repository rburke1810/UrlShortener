using Database.Entities;
using Database.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class UrlDetailRepository : IUrlDetailRepository
    {
        private readonly UrlShortenerContext _context;
        public UrlDetailRepository(UrlShortenerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UrlDetail> AddAsync(UrlDetail urlDetail)
        {
            _context.UrlDetails.Add(urlDetail);
            await _context.SaveChangesAsync();
            return urlDetail;
        }

        public UrlDetail GetByOriginalUrl(string originalUrl)
        {
            var urlDetail = _context.UrlDetails.Where(u => u.OriginalUrl == originalUrl).FirstOrDefault();
            return urlDetail;
        }

        public UrlDetail GetByCode(string code)
        {
            var urlDetail = _context.UrlDetails.Where(u => u.Code == code).FirstOrDefault();
            return urlDetail;
        }
    }
}
