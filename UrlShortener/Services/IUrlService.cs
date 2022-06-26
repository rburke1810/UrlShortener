using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<string> CreateShortUrlAsync(string longUrl);
        string GetOriginalUrl(string code);
        bool IsValidUrl(string url);
    }
}
