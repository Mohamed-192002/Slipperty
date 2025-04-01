using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    public class OrdersController : Controller
    {
        public OrdersController()
        {
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
