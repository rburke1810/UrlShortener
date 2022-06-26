using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<UrlDetail> UrlDetails { get; set; }
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options)
        {

        }
    }
}
