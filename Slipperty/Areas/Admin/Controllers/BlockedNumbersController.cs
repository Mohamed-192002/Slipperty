using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class BlockedNumbersController : BaseController
    {
        private readonly IBlockedNumbersBusiness _BlockedNumbersBusiness;
        private readonly ICategoriesBusiness _categoriesBusiness;

        public BlockedNumbersController(IBlockedNumbersBusiness BlockedNumbersBusiness, ICategoriesBusiness categoriesBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _BlockedNumbersBusiness = BlockedNumbersBusiness;
            _categoriesBusiness = categoriesBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<BlockedNumberDTO> DataList = _BlockedNumbersBusiness.GetAll().OrderBy(u => u.Id).ToList();

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
            populateViewBags();
            var data = new BlockedNumberDTO();
            if (id > 0)
            {
                data = _BlockedNumbersBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(BlockedNumberDTO modelDTO)
        {
            if (ModelState.IsValid)
            {
                var result = 0;
                
                result = _BlockedNumbersBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            populateViewBags();
            var model = new BlockedNumberDTO();
            if (id > 0)
            {
                model = _BlockedNumbersBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(BlockedNumberDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "BlockedNumberId");

        //        bool result = _BlockedNumbersBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
                
        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(BlockedNumberDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "BlockedNumberId");

                bool result = _BlockedNumbersBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
                
                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }

        public void populateViewBags()
        {
            ViewBag.Categories = setSelectList(_categoriesBusiness.GetAll().ToList(), "Id", "Name");
        }
    }
}
