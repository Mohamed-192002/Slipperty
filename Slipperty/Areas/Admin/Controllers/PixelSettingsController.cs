using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class PixelSettingsController : BaseController
    {
        private readonly IPixelSettingsBusiness _PixelSettingsBusiness;

        public PixelSettingsController(IPixelSettingsBusiness PixelSettingsBusiness, ICategoriesBusiness categoriesBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _PixelSettingsBusiness = PixelSettingsBusiness;
        }

        public IActionResult Index()
        {
            PixelSettingDTO Data = _PixelSettingsBusiness.GetAll()?.FirstOrDefault();
            return View(Data);
        }

        

        [HttpPost]
        public IActionResult Index(PixelSettingDTO modelDTO)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                result = _PixelSettingsBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), modelDTO);
        }
    }
}
