using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlService _urlService;
        public HomeController(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Shorten(string originalUrl)
        {
            if(!_urlService.IsValidUrl(originalUrl))
            {
                ViewBag.Error = $"{originalUrl} is an invalid URL.";
                return View("Index");
            }
            var result = await _urlService.CreateShortUrlAsync(originalUrl);
            ViewBag.LongUrl = originalUrl;
            ViewBag.ShortUrl = result;
            return View("Index");
        }
    }
}
