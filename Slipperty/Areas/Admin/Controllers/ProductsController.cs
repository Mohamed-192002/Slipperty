using AspNetCore.Reporting;
using Business.DTO;
using Infrastructure.Contracts.Seeds;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using static Business.Helpers.BusinessHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slipperty.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class ProductsController : BaseController
    {
        private readonly IProductsBusiness _ProductsBusiness;
        private readonly ICategoriesBusiness _categoriesBusiness;
        private readonly ISubCategoriesBusiness _subCategoriesBusiness;
        private readonly IProductTypesBusiness _productTypesBusiness;
        private readonly IMaterialsBusiness _materialsBusiness;
        private readonly IBrandsBusiness _brandsBusiness;
        private readonly IManufacturingBusiness _manufacturingBusiness;
        private readonly IReportsBusiness _reportsBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductsBusiness ProductsBusiness, ICategoriesBusiness categoriesBusiness, ISubCategoriesBusiness subCategoriesBusiness,
            IProductTypesBusiness productTypesBusiness,IMaterialsBusiness materialsBusiness, IBrandsBusiness brandsBusiness, IPixelSettingsBusiness pixelSettingsBusiness,
            IManufacturingBusiness manufacturingBusiness, IReportsBusiness  reportsBusiness, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _ProductsBusiness = ProductsBusiness;
            _categoriesBusiness = categoriesBusiness;
            _subCategoriesBusiness = subCategoriesBusiness;
            _productTypesBusiness = productTypesBusiness;
            _materialsBusiness = materialsBusiness;
            _brandsBusiness = brandsBusiness;
            _manufacturingBusiness = manufacturingBusiness;
            _reportsBusiness = reportsBusiness;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int? page)
        {
            var productQuery = _ProductsBusiness.GetAll();

            //Paginantion
            const int pageSize = 25; // Number of items per page
            int pageNumber = (page ?? 1);
            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((double)productQuery.Count() / pageSize);
            totalPages = totalPages == 0 ? 1 : totalPages;

            // Paginate the data
            var DataList = productQuery.OrderBy(u => u.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            //Get Displayed List Count
            int DisplayedItemsCount = DataList.Count();

            SetPagination(pageSize, pageNumber, totalPages, DisplayedItemsCount);

            return View(DataList);
        }

        public IActionResult Upsert(int? id)
        {
            populateViewBags();
            var data = new ProductDTO();
            if (id > 0)
            {
                data = _ProductsBusiness.GetById(Convert.ToInt32(id));

                if(data.ProductImages != null)
                {
                    data.ProductImages = data.ProductImages.OrderBy(u => u.DisplayOrder).ToList();
                    //data.ProductImagesOrder = data.ProductImagesOrder.OrderBy(u => u.DisplayOrder).ToList();
                }

                //Recalculate Duration
                DateTime currentDate = DateTime.Now;

                // Calculate the time difference between the current date and the product's RegDate
                TimeSpan timeDifference = currentDate - data.RegDate.Value;

                // Calculate the remaining duration in minutes
                // Convert the total minutes to an integer (rounding down)
                //int remainingDuration = (int)data.Duration - (int)timeDifference.TotalMinutes;
                int remainingDuration = 0;

                if (data.Duration.HasValue)
                    remainingDuration = (int)data.Duration.Value - (int)timeDifference.TotalMinutes;

                // If the remaining duration is negative, set it to 0
                remainingDuration = remainingDuration < 0 ? 0 : remainingDuration;

                // Store the remaining duration or use it as needed
                data.Duration = remainingDuration;



                var subcategories = _subCategoriesBusiness.GetAll()
                .Where(sc => data.SelectedCategories.Contains((int)sc.CategoryId)) // Filter subcategories by selected categories
                .Select(sc => new { sc.Id, sc.Name }) // Select only required fields
                .ToList();

                ViewBag.SubCategories = setSelectList(subcategories, "Id", "Name");
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Upsert(ProductDTO modelDTO, List<IFormFile> files)
        {
            //if (modelDTO.Categories == null)
            //modelDTO.Categories = new List<ProductCategoryDTO>();
            //if (modelDTO.SubCategories == null)
            //modelDTO.SubCategories = new List<ProductSubCategoryDTO>();


            //if (modelDTO.Id == 0 || modelDTO.Id == null)
            //{
            //if (modelDTO.SelectedCategories != null && modelDTO.SelectedCategories.Count() > 0)
            //    modelDTO.Categories = modelDTO.Categories.Concat(modelDTO?.SelectedCategories?.Select(categoryId => new ProductCategoryDTO { CategoryId = categoryId })).ToList();
            //if (modelDTO.SelectedSubCategories != null && modelDTO.SelectedSubCategories.Count() > 0)
            //    modelDTO.SubCategories = modelDTO.SubCategories.Concat(modelDTO.SelectedSubCategories.Select(subCategoryId => new ProductSubCategoryDTO { SubCategoryId = subCategoryId })).ToList();

            //}
            //else
            //{
            //    //Update
            //    //var currentproduct = _ProductsBusiness.GetById((int)modelDTO.Id);
            //    modelDTO.Categories = currentproduct.Categories;
            //    modelDTO.SubCategories = currentproduct.SubCategories;


            //}

            if (modelDTO.SelectedCategories != null)
            {
                modelDTO.Categories = modelDTO.SelectedCategories.Select(categoryId => new ProductCategoryDTO { CategoryId = categoryId }).ToList();
            }

            if (modelDTO.SelectedSubCategories != null)
            {
                modelDTO.SubCategories = modelDTO.SelectedSubCategories.Select(subCategoryId => new ProductSubCategoryDTO { SubCategoryId = subCategoryId }).ToList();
            }

            if (modelDTO.ProductVariables != null && modelDTO.ProductVariables.Count() > 0)
            {
                foreach (var variable in modelDTO.ProductVariables)
                {
                    variable.VariableValues = modelDTO?.ProductVariableValues?.Where(c => c.VariableName == variable.Name).ToList();
                }
            }
            //if (modelDTO.SelectedCategories != null && modelDTO.SelectedCategories.Count() > 0)
            //{
            //    modelDTO.Categories = modelDTO.Categories ?? new List<ProductCategoryDTO>(); // Initialize if null
            //    modelDTO.Categories.AddRange(modelDTO.SelectedCategories.Select(categoryId => new ProductCategoryDTO { CategoryId = categoryId }));
            //}

            //if (modelDTO.SelectedSubCategories != null && modelDTO.SelectedSubCategories.Count() > 0)
            //{
            //    modelDTO.SubCategories = modelDTO.SubCategories ?? new List<ProductSubCategoryDTO>(); // Initialize if null
            //    modelDTO.SubCategories.AddRange(modelDTO.SelectedSubCategories.Select(subCategoryId => new ProductSubCategoryDTO { SubCategoryId = subCategoryId }));
            //}


            //foreach (var categoryId in modelDTO?.SelectedCategories)
            //{
            //    modelDTO.Categories.Add(new ProductCategoryDTO { CategoryId = categoryId });
            //}

            //foreach (var subCategoryId in modelDTO?.SelectedSubCategories)
            //{
            //    modelDTO.SubCategories.Add(new ProductSubCategoryDTO { SubCategoryId = subCategoryId });
            //}
            if (ModelState.IsValid)
            {
                var result = 0;

                modelDTO.RegDate = DateTime.Now;
                if(modelDTO.VariableCombinations != null)
                    modelDTO.StockCount = (int)modelDTO.VariableCombinations.Sum(c => c.StockCount);
                else
                modelDTO.StockCount = 0;

                if (modelDTO.ProductImages == null)
                {
                    modelDTO.ProductImages = new List<ProductImageDTO>();
                }

                foreach (IFormFile file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductsPath = ImagesPathes.Products;
                    string finalPath = Path.Combine(getWwwRootPath(), ProductsPath);
                    saveImage(finalPath, fileName, file);

                    if (modelDTO.MainImageUrl == file.FileName)
                        modelDTO.MainImageUrl = fileName;
                    if (modelDTO.IconImageUrl == file.FileName)
                        modelDTO.IconImageUrl = fileName;


                    ProductImageDTO productImage = new()
                    {
                        ImageUrl = fileName
                    };

                    if(modelDTO.ProductVariableValues != null && modelDTO.ProductVariableValues.Count() > 0)
                    {
                        var variableValue = modelDTO.ProductVariableValues.FirstOrDefault(c => c.ImageUrl == file.FileName);
                        if(variableValue != null)
                        {
                            //var variable = modelDTO.ProductVariables.FirstOrDefault(c => c.Name == variableValue.VariableName).VariableValues.FirstOrDefault(c => c.ImageUrl == variableValue.ImageUrl);
                            //if(variable != null)
                            //{
                                modelDTO.ProductVariables.FirstOrDefault(c => c.Name == variableValue.VariableName).VariableValues.FirstOrDefault(c => c.ImageUrl == variableValue.ImageUrl).ImageUrl = fileName;
                            //}
                        }
                    }


                    modelDTO.ProductImages.Add(productImage);
                }


                foreach (var productImage in modelDTO.ProductImages)
                {
                    var image = modelDTO.ProductImagesOrder.Where(c => c.ImageUrl == productImage.ImageUrl).FirstOrDefault();
                    if(image != null)
                        productImage.DisplayOrder = modelDTO.ProductImagesOrder.Where(c => c.ImageUrl == productImage.ImageUrl).FirstOrDefault().DisplayOrder;
                }


                if (modelDTO.ProductImages != null && modelDTO.ProductImages.Any())
                {
                    var isCorrectMainImage = modelDTO.ProductImages.Where(c => c.ImageUrl == modelDTO.MainImageUrl).FirstOrDefault();
                    var isCorrectIconImage = modelDTO.ProductImages.Where(c => c.ImageUrl == modelDTO.IconImageUrl).FirstOrDefault();
                    if (isCorrectMainImage == null)
                        modelDTO.MainImageUrl = modelDTO.ProductImages.FirstOrDefault()?.ImageUrl;
                    if (isCorrectIconImage == null)
                        modelDTO.IconImageUrl = modelDTO.ProductImages.FirstOrDefault()?.ImageUrl;
                }


                if (modelDTO.ProductImages == null || !modelDTO.ProductImages.Any())
                {
                        modelDTO.MainImageUrl = "";
                        modelDTO.IconImageUrl = "";
                }

                if(modelDTO.ProductVariables != null)
                {
                    //Set Color Image If null
                    foreach (var productVariable in modelDTO.ProductVariables.Where(c => c.Name == "اللون" || c.Name == "الالوان" || c.Name == "لون" || c.Name == "الوان" || c.Name == "الألوان" || c.Name == "ألوان"))
                    {
                        foreach (var value in productVariable.VariableValues)
                        {
                            if (string.IsNullOrEmpty(value.ImageUrl))
                                value.ImageUrl = modelDTO.MainImageUrl;
                        }
                    }
                }

                

                result = _ProductsBusiness.AddUpdate(modelDTO);

                setUpsertTempMessages(result);
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelStateInvalid"] = true;

            var subcategories = _subCategoriesBusiness.GetAll()
            .Where(sc => modelDTO.SelectedCategories.Contains((int)sc.CategoryId)) // Filter subcategories by selected categories
            .Select(sc => new { sc.Id, sc.Name }) // Select only required fields
            .ToList();

            ViewBag.SubCategories = setSelectList(subcategories, "Id", "Name");

            populateViewBags();
            //ViewBag.SubCategories = setSelectList(_subCategoriesBusiness.GetAll().Where(c => c.CategoryId == modelDTO.CategoryId).ToList(), "Id", "Name");
            return View(nameof(Upsert), modelDTO);
        }


        public IActionResult Delete(int id)
        {
            populateViewBags();
            var model = new ProductDTO();
            if (id > 0)
            {
                model = _ProductsBusiness.GetById(id);
                //ViewBag.SubCategories = setSelectList(_subCategoriesBusiness.GetAll().Where(c => c.CategoryId == model.CategoryId).ToList(), "Id", "Name");
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Delete(ProductDTO modelDTO)
        //{
        //    if (modelDTO?.Id > 0)
        //    {
        //        //bool inRelation = hasRelation(modelDTO.Id.ToString(), "ProductId");
        //        bool inRelation = false;
        //        //ToDo: check in  orders before delete

        //        bool result = _ProductsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

        //        //string ProductsPath = ImagesPathes.Products;
        //        //string finalPath = Path.Combine(getWwwRootPath(), ProductsPath, modelDTO?.ImageUrl);

        //        //deleteImage(finalPath);

        //        setDeleteMessages(result);


        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public IActionResult Delete(ProductDTO modelDTO)
        {
            if (modelDTO?.Id > 0)
            {
                bool inRelation = hasRelation(modelDTO.Id.ToString(), "ProductId");
                //bool inRelation = false;
                //ToDo: check in  orders before delete

                bool result = _ProductsBusiness.Delete(Convert.ToInt32(modelDTO.Id), inRelation);

                //string ProductsPath = ImagesPathes.Products;
                //string finalPath = Path.Combine(getWwwRootPath(), ProductsPath, modelDTO?.ImageUrl);

                //deleteImage(finalPath);

                setDeleteMessages(result);


                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid ID." });
        }

        public void populateViewBags()
        {
            ViewBag.Categories = setSelectList(_categoriesBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.ProductTypes = setSelectList(_productTypesBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Materials = setSelectList(_materialsBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Brands = setSelectList(_brandsBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Manufacturings = setSelectList(_manufacturingBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Products = setSelectList(_ProductsBusiness.GetAll().ToList(), "Id", "ArbName");
            //ViewBag.SubCategories = setSelectList(_subCategoriesBusiness.GetAll().ToList(), "Id", "Name");
        }

        //public IActionResult GetSubCategories(int categoryId)
        //{
        //    var subCategories = _subCategoriesBusiness.GetAll().Where(c => c.CategoryId == categoryId).Select(subCategory => new
        //    {
        //        id = subCategory.Id,
        //        text = subCategory.Name
        //    }).ToList();

        //    return Json(new { subCategories = subCategories });
        //}

        [HttpGet]
        public IActionResult GetSubCategories(List<int> categoryIds)
        {
            if (categoryIds == null || !categoryIds.Any())
            {
                return Json(new List<object>());
            }

            // Fetch the subcategories related to the selected categories
            var subcategories = _subCategoriesBusiness.GetAll()
                .Where(sc => categoryIds.Contains((int)sc.CategoryId)) // Filter subcategories by selected categories
                .Select(sc => new { sc.Id, sc.Name }) // Select only required fields
                .ToList();

            // Return the subcategories as a JSON response
            return Json(subcategories);
        }


        public IActionResult GetVariableRow(string VariableName)
        {
            var Variable = new ProductVariableDTO
            {
                Name = VariableName
            };
            return PartialView("_VariablesFormPartial", Variable);
        }
        
        public IActionResult AddImagesToTable(string imageName)
        {
            var image = new ProductImageOrderDTO
            {
                ImageUrl = imageName,
                isSaved = false
            };
            return PartialView("_ImagesOrderFormPartial", image);
        }
        //public IActionResult GetVariableValueRow(int variableId, string variableName, string variableValue)
        //{
        //    var Variable = new VariableValueDTO
        //    {
        //        VariableName = variableName,
        //        ProductVariableId = variableId,
        //        Value = variableValue

        //    };
        //    return PartialView("_VariableValuesFormPartial", Variable);
        //}

        public IActionResult GetVariableValueRow(string variableName, int variableId)
        {
            var Variable = new VariableValueDTO
            {
                VariableName = variableName,
                ProductVariableId = variableId

            };
            return PartialView("_VariableValuesFormPartial", Variable);
        }

        public IActionResult GetVariableCombinationRow(string CombinationText, decimal price)
        {
            // Prepare the DTO for the row
            var combinationDTO = new VariableCombinationDTO();
            combinationDTO.Text = CombinationText;
            combinationDTO.Price = price;


            // Return the partial view with the dynamic DTO
            return PartialView("_VariableCombinationFormPartial", combinationDTO);
        }



        public IActionResult GetCountsOfferRow(int count, decimal price, decimal itemPrice)
        {
            var offer = new ProductCountsOfferDTO
            {
                Price = price,
                Count = count

            };
            return PartialView("_ProductOffersFormPartial", offer);
        }

        public IActionResult GetRelatedProductRow(int productId)
        {
            var RelatedProduct = new RelatedProductDTO
            {
                ProductId = null,
                RelatedProductId = productId,
                Product = _ProductsBusiness.GetById(productId)

            };
            return PartialView("_RelatedProductsFormPartial", RelatedProduct);
        }

        public IActionResult GetvideoUrlRow(string videoUrl)
        {
            var video = new ProductVideoDTO
            {
                VideoUrl = videoUrl

            };
            return PartialView("_VideosFormPartial", video);
        }

        [HttpGet]
        public IActionResult GetRelatedProducts(List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return Json(new List<object>());
            }

            var products = _ProductsBusiness.GetAll()
                .Where(sc => !productIds.Contains((int)sc.Id))
                .Select(sc => new { sc.Id, sc.ArbName }) // Select only required fields
                .ToList();

            // Return the subcategories as a JSON response
            return Json(products);
        }

        [HttpPost]
        public JsonResult AddNewBrand(string newValue)
        {
            // Validate the input
            if (string.IsNullOrEmpty(newValue))
            {
                return Json(new { success = false, message = "ادخل الاسم" });
            }

            try
            {
                // Create the new brand
                BrandDTO brand = new BrandDTO
                {
                    Name = newValue,
                    RegDate = DateTime.UtcNow
                };

                // Add or update the brand
                var result = _brandsBusiness.AddUpdate(brand);
                if (result == 1)
                {
                    // Get the updated list of brands
                    var brands = _brandsBusiness.GetAll(); // This should return a list of BrandDTOs with Id and Name

                    // Return the updated list as a JSON response
                    return Json(new { success = true, brands = brands });
                }

                // If the operation fails, return an error
                return Json(new { success = false, message = "حدث خطأ" });
            }
            catch (Exception ex)
            {
                // Handle exceptions and log them (optional)
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddNewMaterial(string newValue)
        {
            // Validate the input
            if (string.IsNullOrEmpty(newValue))
            {
                return Json(new { success = false, message = "ادخل الاسم" });
            }

            try
            {
                // Create the new material
                MaterialDTO material = new MaterialDTO
                {
                    Name = newValue,
                    RegDate = DateTime.UtcNow
                };

                // Add or update the material
                var result = _materialsBusiness.AddUpdate(material);  // Assuming this method exists in your business layer
                if (result == 1)
                {
                    // Get the updated list of materials
                    var materials = _materialsBusiness.GetAll();  // This should return a list of MaterialDTOs with Id and Name

                    // Return the updated list as a JSON response
                    return Json(new { success = true, materials = materials });
                }

                // If the operation fails, return an error
                return Json(new { success = false, message = "حدث خطأ" });
            }
            catch (Exception ex)
            {
                // Handle exceptions and log them (optional)
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddNewProductType(string newValue)
        {
            // Validate the input
            if (string.IsNullOrEmpty(newValue))
            {
                return Json(new { success = false, message = "ادخل الاسم" });
            }

            try
            {
                // Create the new product type
                ProductTypeDTO productType = new ProductTypeDTO
                {
                    Name = newValue,
                    RegDate = DateTime.UtcNow
                };

                // Add or update the product type (assume AddUpdate is a method in your business layer)
                var result = _productTypesBusiness.AddUpdate(productType);
                if (result == 1)
                {
                    // Get the updated list of product types
                    var productTypes = _productTypesBusiness.GetAll(); // This should return a list of ProductTypeDTOs with Id and Name

                    // Return the updated list as a JSON response
                    return Json(new { success = true, productTypes = productTypes });
                }

                // If the operation fails, return an error
                return Json(new { success = false, message = "حدث خطأ" });
            }
            catch (Exception ex)
            {
                // Handle exceptions and log them (optional)
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddNewManufacturing(string newValue)
        {
            // Validate the input
            if (string.IsNullOrEmpty(newValue))
            {
                return Json(new { success = false, message = "الرجاء إدخال اسم النوع" });
            }

            try
            {
                // Create the new manufacturing type
                ManufacturingDTO manufacturing = new ManufacturingDTO
                {
                    Name = newValue,
                    RegDate = DateTime.UtcNow
                };

                // Add or update the manufacturing type (assume AddUpdate is a method in your business layer)
                var result = _manufacturingBusiness.AddUpdate(manufacturing);
                if (result == 1)
                {
                    // Get the updated list of manufacturing types
                    var manufacturings = _manufacturingBusiness.GetAll(); // This should return a list of ManufacturingDTOs with Id and Name

                    // Return the updated list as a JSON response
                    return Json(new { success = true, manufacturings = manufacturings });
                }

                // If the operation fails, return an error
                return Json(new { success = false, message = "حدث خطأ" });
            }
            catch (Exception ex)
            {
                // Handle exceptions and log them (optional)
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult PrintBarcode(string text, string productName)
        {
            var formattedText = FormatProductDetails(text);
            DataTable BarcodeDT = new DataTable();
            BarcodeDT = _reportsBusiness.GetBarcodeDT(formattedText, productName);
            string mimeType = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Barcode.rdlc";


            LocalReport report = new LocalReport(path);
            report.AddDataSource("BarcodeDataSet", BarcodeDT); ;
            var result = report.Execute(RenderType.Pdf, extension, null, mimeType);
            return File(result.MainStream, "application/pdf");
        }


        //private string FormatProductDetails(string text)
        //{
        //    // Define regex pattern to extract all values after ":" and before ","
        //    var pattern = @"(?:\S+:\s*)([^,]+)"; // Matches "key: value" and captures the value

        //    // Use regex to find all values
        //    var matches = Regex.Matches(text, pattern);

        //    // Extract values and join them with " - "
        //    var formattedText = string.Join(" - ", matches.Cast<Match>().Select(m => m.Groups[1].Value));

        //    return formattedText;
        //}

    }
}
