using Database.Entities;
using Database.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Tests.Unit.Services
{
    public class UrlServiceTests
    {
        private IUrlService _urlService;
        private Mock<IUrlDetailRepository> _mockUrlDetailRepository;
        private string originalUrl;
        private string code;
        private UrlDetail urlDetail;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "DomainPrefix", "http://domainprefix.com" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            originalUrl = "http://shorturl.com";
            code = "abc123";
            urlDetail = new UrlDetail
            {
                OriginalUrl = originalUrl,
                Code = code
            };

            _mockUrlDetailRepository = new Mock<IUrlDetailRepository>();
            _urlService = new UrlService(_mockUrlDetailRepository.Object, configuration);
        }

        [Test]
        public async Task CreateShortUrl_ShouldReturnExistingCodeIfExists()
        {
            _mockUrlDetailRepository.Setup(s => s.GetByOriginalUrl(originalUrl)).Returns(urlDetail);

            var result = await _urlService.CreateShortUrlAsync(originalUrl);

            result.Should().Be($"http://domainprefix.com/{urlDetail.Code}");

            _mockUrlDetailRepository.Verify(s => s.AddAsync(It.IsAny<UrlDetail>()), Times.Never);
        }

        [Test]
        public async Task CreateShortUrl_ShouldReturnShortUrl()
        {
            _mockUrlDetailRepository.Setup(s => s.AddAsync(It.IsAny<UrlDetail>())).ReturnsAsync(urlDetail);

            var result = await _urlService.CreateShortUrlAsync("https://newsite.com");

            result.Should().Be($"http://domainprefix.com/{urlDetail.Code}");
            _mockUrlDetailRepository.Verify(s => s.AddAsync(It.IsAny<UrlDetail>()), Times.Once);
        }

        [Test]
        public void CreateShortUrl_ShouldGenerateNewCodeIfGeneratedCodeExists()
        {
            _mockUrlDetailRepository.SetupSequence(s => s.GetByCode(It.IsAny<string>())).Returns(urlDetail).Returns(urlDetail).Returns(value: null);

            var result = _urlService.CreateShortUrlAsync("https://newsite.com");

            _mockUrlDetailRepository.Verify(s => s.GetByCode(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test]
        public void GetOriginalUrl_ShouldReturnOriginalUrl()
        {
            _mockUrlDetailRepository.Setup(s => s.GetByCode(code)).Returns(urlDetail);

            var result = _urlService.GetOriginalUrl(code);

            result.Should().Be(urlDetail.OriginalUrl);
            _mockUrlDetailRepository.Verify(s => s.GetByCode(It.IsAny<string>()), Times.Once);
        }
    }
}
