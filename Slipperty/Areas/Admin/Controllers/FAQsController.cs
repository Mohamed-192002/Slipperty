using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class FAQsController : BaseController
    {
        private readonly IFAQsBusiness _FAQsBusiness;

        public FAQsController(IFAQsBusiness FAQsBusiness, ICategoriesBusiness categoriesBusiness, UserManager<ApplicationUser> userManager, 
            IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _FAQsBusiness = FAQsBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<FAQDTO> DataList = _FAQsBusiness.GetAll().OrderBy(u => u.Id).ToList();
            
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
            var data = new FAQDTO();
            if (id > 0)
            {
                data = _FAQsBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(FAQDTO modelDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;

                result = _FAQsBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            var model = new FAQDTO();
            if (id > 0)
            {
                model = _FAQsBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(FAQDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "FAQId");

        //        bool result = _FAQsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(FAQDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "FAQId");

                bool result = _FAQsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }
    }
}
