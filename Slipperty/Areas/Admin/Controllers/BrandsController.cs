using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class BrandsController : BaseController
    {
        private readonly IBrandsBusiness _BrandsBusiness;

        public BrandsController(IBrandsBusiness BrandsBusiness, ICategoriesBusiness categoriesBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _BrandsBusiness = BrandsBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<BrandDTO> DataList = _BrandsBusiness.GetAll().OrderBy(u => u.Id).ToList();
            
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

            SetPagination(pageSize, pageNumber,totalPages, DisplayedItemsCount);
            
            return View(DataList);
        }

        public IActionResult Upsert(int? id)
        {
            var data = new BrandDTO();
            if (id > 0)
            {
                data = _BrandsBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(BrandDTO modelDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;

                result = _BrandsBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            var model = new BrandDTO();
            if (id > 0)
            {
                model = _BrandsBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(BrandDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "BrandId");

        //        bool result = _BrandsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(BrandDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "BrandId");

                bool result = _BrandsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }
    }
}
