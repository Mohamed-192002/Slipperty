using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class LinksController : BaseController
    {
        private readonly ILinksBusiness _LinksBusiness;
        private readonly ILinkTypesBusiness _linkTypesBusiness;

        public LinksController(ILinksBusiness LinksBusiness, ILinkTypesBusiness linkTypesBusiness, ICategoriesBusiness categoriesBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _LinksBusiness = LinksBusiness;
            _linkTypesBusiness = linkTypesBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<LinkDTO> DataList = _LinksBusiness.GetAll().OrderBy(u => u.Id).ToList();
            
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
            populateViewBags();
            var data = new LinkDTO();
            if (id > 0)
            {
                data = _LinksBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(LinkDTO modelDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;
                
                result = _LinksBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            populateViewBags();
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            populateViewBags();
            var model = new LinkDTO();
            if (id > 0)
            {
                model = _LinksBusiness.GetById(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(LinkDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "LinkId");

                bool result = _LinksBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
               
                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }

        public void populateViewBags()
        {
            ViewBag.LinkTypes = setSelectList(_linkTypesBusiness.GetAll().ToList(), "Id", "Name");
        }
    }
}
