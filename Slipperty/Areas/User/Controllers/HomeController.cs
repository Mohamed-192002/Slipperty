using Microsoft.AspNetCore.Mvc;
using Slipperty.Models;
using System.Diagnostics;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoriesBusiness _categoriesBusiness;
        private readonly IProductsBusiness _productsBusiness;
        private readonly IBannersBusiness _bannersBusiness;
        private readonly ILinksBusiness _linksBusiness;

        public HomeController(ILogger<HomeController> logger, ICategoriesBusiness categoriesBusiness, IProductsBusiness productsBusiness, IBannersBusiness bannersBusiness, ILinksBusiness linksBusiness)
        {
            _logger = logger;
            _categoriesBusiness = categoriesBusiness;
            _productsBusiness = productsBusiness;
            _bannersBusiness = bannersBusiness;
            _linksBusiness = linksBusiness;
        }

        public IActionResult Index()
        {
           

            var products = _productsBusiness.GetAll();
            HomePageDTO homePageDTO = new HomePageDTO();
            homePageDTO.Categories = _categoriesBusiness.GetAll();
            homePageDTO.RecenntlyArrived = products.OrderBy(c=>c.RegDate).Take(4);
            homePageDTO.HighlyRated = products.OrderBy(c=>c.RegDate).Take(4);
            homePageDTO.Banners = _bannersBusiness.GetAll().OrderBy(c=>c.RegDate).Take(4);
            
            return View(homePageDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            ContactUsDTO contactUsDTO = new ContactUsDTO();
            contactUsDTO.Links = _linksBusiness.GetAll();
            return View(contactUsDTO);
        }

        


        public IActionResult GetFilteredProducts(int categoryId)
        {
            var filteredProducts = _productsBusiness.GetAll();
            if(categoryId == 0)
                filteredProducts = filteredProducts.OrderBy(c => c.RegDate).Take(4);
            else
                filteredProducts = filteredProducts.Where(c => c.Categories.Any(cat => cat.CategoryId == categoryId));

            filteredProducts.OrderBy(c => c.RegDate).Take(4).ToList();

            // Return the partial view with the filtered products
            return PartialView("_FilteredProductsPartial", filteredProducts);
        }




    }
}
