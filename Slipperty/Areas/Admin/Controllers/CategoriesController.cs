using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesBusiness _categoriesBusiness;

        public CategoriesController(ICategoriesBusiness categoriesBusiness, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _categoriesBusiness = categoriesBusiness;
        }

        public IActionResult Index(int? page)
        {
            List<CategoryDTO> DataList = _categoriesBusiness.GetAll().OrderBy(u => u.Id).ToList();
            
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
            var data = new CategoryDTO();
            if (id > 0)
            {
                data = _categoriesBusiness.GetById(Convert.ToInt32(id));
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(CategoryDTO modelDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;


                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoriesPath = ImagesPathes.Categories;
                    string finalPath = Path.Combine(getWwwRootPath(), categoriesPath);
                    saveImage(finalPath, fileName, file);
                    modelDTO.ImageUrl = fileName;
                }

                result = _categoriesBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            var model = new CategoryDTO();
            if (id > 0)
            {
                model = _categoriesBusiness.GetById(id);
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(CategoryDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        bool inRelation = hasRelation(modelDTO.Id.ToString(), "CategoryId");

        //        bool result = _categoriesBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
        //        if(modelDTO?.ImageUrl != null)
        //        {

        //            string categoriesPath = ImagesPathes.Categories;
        //            string finalPath = Path.Combine(getWwwRootPath(), categoriesPath, modelDTO?.ImageUrl);

        //            deleteImage(finalPath);
        //        }

        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(CategoryDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "CategoryId");

                bool result = _categoriesBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);
                if(modelDTO?.ImageUrl != null)
                {

                    string categoriesPath = ImagesPathes.Categories;
                    string finalPath = Path.Combine(getWwwRootPath(), categoriesPath, modelDTO?.ImageUrl);

                    deleteImage(finalPath);
                }

                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }
    }
}
