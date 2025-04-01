using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class BannersController : BaseController
    {
        private readonly IBannersBusiness _BannersBusiness;

        public BannersController(IBannersBusiness BannersBusiness, ICategoriesBusiness categoriesBusiness, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _BannersBusiness = BannersBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<BannerDTO> DataList = _BannersBusiness.GetAll().OrderBy(u => u.Id).ToList();

            //Paginantion
            const int pageSize = 25; // Number of items per page
            int pageNumber = (page ?? 1);
            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((double)DataList.Count / pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            // Paginate the data
            DataList = DataList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            //Get Displayed List Count
            int DisplayedItemsCount = DataList.Count;

            SetPagination(pageSize, pageNumber, totalPages, DisplayedItemsCount);

            return View(DataList);
        }

        public IActionResult Upsert(int? id)
        {
            var data = new BannerDTO();
            if (id > 0)
            {
                data = _BannersBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(BannerDTO modelDTO, IFormFile? file)
        {

            if (modelDTO.Id == 0 || modelDTO.Id is null)
            {
                if (file is null)
                    ModelState.AddModelError("ImageUrl", "اختر صورة");
            }

            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;


                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string bannersPath = ImagesPathes.Banners;
                    string finalPath = Path.Combine(getWwwRootPath(), bannersPath);
                    saveImage(finalPath, fileName, file);
                    modelDTO.ImageUrl = fileName;
                }


                result = _BannersBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            var model = new BannerDTO();
            if (id > 0)
            {
                model = _BannersBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(BannerDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "BannerId");

        //        bool result = _BannersBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
        //        if (modelDTO?.ImageUrl != null)
        //        {

        //            string bannersPath = ImagesPathes.Banners;
        //            string finalPath = Path.Combine(getWwwRootPath(), bannersPath, modelDTO?.ImageUrl);

        //            deleteImage(finalPath);
        //        }
        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(BannerDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "BannerId");

                bool result = _BannersBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
                if (modelDTO?.ImageUrl != null)
                {

                    string bannersPath = ImagesPathes.Banners;
                    string finalPath = Path.Combine(getWwwRootPath(), bannersPath, modelDTO?.ImageUrl);

                    deleteImage(finalPath);
                }
                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }
    }
}
