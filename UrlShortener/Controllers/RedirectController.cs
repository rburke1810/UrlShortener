using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class RedirectController : Controller
    {
        private readonly IUrlService _urlService;
        public RedirectController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        [Route("/{code}")]
        public IActionResult RedirectToOriginal(string code)
        {
            var result = _urlService.GetOriginalUrl(code);
            if(string.IsNullOrEmpty(result))
            {
                ViewBag.Error = "Oops! That wasn't a valid url in our system.";
                return View("Index");
            }

            return Redirect(result);
        }
    }
}
