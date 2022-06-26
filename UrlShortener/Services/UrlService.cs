using Database.Entities;
using Database.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;


namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlDetailRepository _repository;
        private string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();
        private readonly string _domainPrefix;

        public UrlService(IUrlDetailRepository repository, IConfiguration config)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _domainPrefix = config.GetValue<string>("DomainPrefix") ?? throw new ArgumentNullException(nameof(_domainPrefix));
        }
        public async Task<string> CreateShortUrlAsync(string originalUrl)
        {
            var UrlDetail = _repository.GetByOriginalUrl(originalUrl);
            if (UrlDetail == null)
            {
                var code = GenerateCode();

                var newUrlDetail = new UrlDetail
                {
                    OriginalUrl = originalUrl,
                    Code = code
                };

                UrlDetail = await _repository.AddAsync(newUrlDetail);
            }

            return $"{_domainPrefix}/{UrlDetail.Code}";
        }

        public string GetOriginalUrl(string code)
        {
            var urlDetail = _repository.GetByCode(code);
            return urlDetail?.OriginalUrl;
        }

        private string GenerateCode()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            var code = sb.ToString();

            var urlDetail = _repository.GetByCode(code);
            if (urlDetail != null)
            {
               GenerateCode();
            }
            return code;
        }

        public bool IsValidUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
