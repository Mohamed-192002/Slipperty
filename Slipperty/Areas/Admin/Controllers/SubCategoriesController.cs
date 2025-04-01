using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class SubCategoriesController : BaseController
    {
        private readonly ISubCategoriesBusiness _SubCategoriesBusiness;
        private readonly ICategoriesBusiness _categoriesBusiness;

        public SubCategoriesController(ISubCategoriesBusiness SubCategoriesBusiness, ICategoriesBusiness categoriesBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _SubCategoriesBusiness = SubCategoriesBusiness;
            _categoriesBusiness = categoriesBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<SubCategoryDTO> DataList = _SubCategoriesBusiness.GetAll().OrderBy(u => u.Id).ToList();
            
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
            var data = new SubCategoryDTO();
            if (id > 0)
            {
                data = _SubCategoriesBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(SubCategoryDTO modelDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;


                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string SubCategoriesPath = ImagesPathes.SubCategories;
                    string finalPath = Path.Combine(getWwwRootPath(), SubCategoriesPath);
                    saveImage(finalPath, fileName, file);
                    modelDTO.ImageUrl = fileName;
                }

                result = _SubCategoriesBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            populateViewBags();
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            populateViewBags();
            var model = new SubCategoryDTO();
            if (id > 0)
            {
                model = _SubCategoriesBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(SubCategoryDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "SubCategoryId");

        //        bool result = _SubCategoriesBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
        //        if (modelDTO?.ImageUrl != null)
        //        {
        //            string SubCategoriesPath = ImagesPathes.SubCategories;
        //            string finalPath = Path.Combine(getWwwRootPath(), SubCategoriesPath, modelDTO?.ImageUrl);

        //            deleteImage(finalPath);
        //        }

        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(SubCategoryDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "SubCategoryId");

                bool result = _SubCategoriesBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
                if (modelDTO?.ImageUrl != null)
                {
                    string SubCategoriesPath = ImagesPathes.SubCategories;
                    string finalPath = Path.Combine(getWwwRootPath(), SubCategoriesPath, modelDTO?.ImageUrl);

                    deleteImage(finalPath);
                }

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
