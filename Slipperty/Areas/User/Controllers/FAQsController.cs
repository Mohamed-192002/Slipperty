using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    public class FAQsController : Controller
    {
        private readonly IFAQsBusiness _FAQsBusiness;
        public FAQsController(IFAQsBusiness FAQsBusiness)
        {
            _FAQsBusiness = FAQsBusiness;
        }
        public IActionResult Index()
        {
            var data = _FAQsBusiness.GetAll();
            return View(data);
        }
    }
}
