using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Business.DTO;
using Infrastructure.Consts;
using Infrastructure.Contracts.Seeds;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text;
using static Business.Helpers.BusinessHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    public class ProductsController : BaseController
    {
        private readonly ICategoriesBusiness _categoriesBusiness;
        private readonly IProductsBusiness _productsBusiness;
        private readonly IMaterialsBusiness _materialsBusiness;
        private readonly IProductTypesBusiness _productTypesBusiness;
        private readonly IBrandsBusiness _brandsBusiness;
        private readonly IManufacturingBusiness _manufacturingBusiness;
        private readonly IReviewsBusiness _reviewsBusiness;
        private readonly IShoppingCartsBusiness _shoppingCartsBusiness;
        private readonly IUserAddressesBusiness _userAddressesBusiness;
        private readonly IUserPaymentMethodsBusiness _userPaymentMethodsBusiness;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGovernmentsBusiness _governmentsBusiness;
        private readonly IRegionsBusiness _regionsBusiness;
        private readonly IOrdersBusiness _ordersBusiness;
        private readonly IGoogleSheetBusiness _googleSheetBusiness;

        //private static int visitorCount = 0; // Static variable for simplicity. In production, use a DB.


        public ProductsController(ICategoriesBusiness categoriesBusiness, IProductsBusiness productsBusiness, IMaterialsBusiness materialsBusiness,
            IProductTypesBusiness productTypesBusiness, IBrandsBusiness brandsBusiness, IManufacturingBusiness manufacturingBusiness, IShoppingCartsBusiness shoppingCartsBusiness,
            IReviewsBusiness reviewsBusiness, IUserAddressesBusiness userAddressesBusiness, IUserPaymentMethodsBusiness userPaymentMethodsBusiness,
            IGovernmentsBusiness governmentsBusiness, IRegionsBusiness regionsBusiness, IOrdersBusiness ordersBusiness, IPixelSettingsBusiness pixelSettingsBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IGoogleSheetBusiness googleSheetBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _categoriesBusiness = categoriesBusiness;
            _productsBusiness = productsBusiness;
            _materialsBusiness = materialsBusiness;
            _productTypesBusiness = productTypesBusiness;
            _brandsBusiness = brandsBusiness;
            _manufacturingBusiness = manufacturingBusiness;
            _reviewsBusiness = reviewsBusiness;
            _shoppingCartsBusiness = shoppingCartsBusiness;
            _ordersBusiness = ordersBusiness;
            _userAddressesBusiness = userAddressesBusiness;
            _userPaymentMethodsBusiness = userPaymentMethodsBusiness;
            _governmentsBusiness = governmentsBusiness;
            _regionsBusiness = regionsBusiness;
            _userManager = userManager;
            _googleSheetBusiness = googleSheetBusiness;
        }

        public IActionResult Index(int? categoryid)
        {
            ProductViewDTO productViewDTO = new ProductViewDTO();
            productViewDTO.Categories = _categoriesBusiness.GetAll();
            productViewDTO.Products = _productsBusiness.GetAll();


            productViewDTO.Sizes = GetSizes();

            if (categoryid > 0)
            {
                productViewDTO.Products = productViewDTO.Products.Where(c => c.Categories.Any(cat => cat.CategoryId == categoryid));
            }
            ViewBag.SelectedCategoryId = categoryid;
            populateViewBags();
            return View(productViewDTO);
        }



        public IEnumerable<int> GetSizes()
        {
            // First, get all the product variables from the database
            var productVariables = _productsBusiness.GetAll().SelectMany(c => c.ProductVariables).ToList();  // ToList() forces the data to be retrieved from the database

            // Now, filter variables where the name matches the required values in memory (client-side)
            var sizeVariables = productVariables
                .Where(pv => pv.Name == "المقاس" || pv.Name == "المقاسات" || pv.Name == "مقاس" || pv.Name == "مقاسات")
                .ToList();  // Apply the filter client-side

            // Extract unique variable values for these filtered variables
            var sizeValues = sizeVariables
                .SelectMany(pv => pv.VariableValues) // Flatten the variable values
                .DistinctBy(vv => vv.Value) // Ensure uniqueness based on the 'Value' field
                .Where(vv => !string.IsNullOrEmpty(vv.Value)) // Filter out null or empty values
                .Select(vv =>
                {
                    // Try to convert the Value to an integer
                    int parsedValue;
                    return int.TryParse(vv.Value, out parsedValue) ? (int?)parsedValue : null;
                })
                .Where(id => id.HasValue) // Filter out null values
                .Select(id => id.Value) // Convert to non-nullable int
                .ToList();  // Convert to list client-side

            return sizeValues;
        }

        public IEnumerable<int> GetSizesForProduct(int productId)
        {
            // First, get all the product variables from the database
            var productVariables = _productsBusiness.GetAll().Where(c => c.Id == productId).SelectMany(c => c.ProductVariables).ToList();  // ToList() forces the data to be retrieved from the database

            // Now, filter variables where the name matches the required values in memory (client-side)
            var sizeVariables = productVariables
                .Where(pv => pv.Name == "المقاس" || pv.Name == "المقاسات" || pv.Name == "مقاس" || pv.Name == "مقاسات")
                .ToList();  // Apply the filter client-side

            // Extract unique variable values for these filtered variables
            var sizeValues = sizeVariables
                .SelectMany(pv => pv.VariableValues) // Flatten the variable values
                .DistinctBy(vv => vv.Value) // Ensure uniqueness based on the 'Value' field
                .Where(vv => !string.IsNullOrEmpty(vv.Value)) // Filter out null or empty values
                .Select(vv =>
                {
                    // Try to convert the Value to an integer
                    int parsedValue;
                    return int.TryParse(vv.Value, out parsedValue) ? (int?)parsedValue : null;
                })
                .Where(id => id.HasValue) // Filter out null values
                .Select(id => id.Value) // Convert to non-nullable int
                .ToList();  // Convert to list client-side

            return sizeValues;
        }


        public IEnumerable<dynamic> GetColors()
        {
            // First, get all the product variables from the database
            var productVariables = _productsBusiness.GetAll().SelectMany(c => c.ProductVariables).ToList();  // ToList() forces the data to be retrieved from the database

            // Now, filter variables where the name matches the required color-related values in memory (client-side)
            var colorVariables = productVariables
                .Where(pv => pv.Name == "اللون" || pv.Name == "الالوان" || pv.Name == "لون" || pv.Name == "الوان" || pv.Name == "الألوان" || pv.Name == "ألوان")
                .ToList();  // Apply the filter client-side

            // Extract unique variable values for these filtered variables
            var colorValues = colorVariables
                .SelectMany(pv => pv.VariableValues) // Flatten the variable values
                .DistinctBy(vv => vv.Value) // Ensure uniqueness based on the 'Value' field
                .Where(vv => !string.IsNullOrEmpty(vv.Value)) // Filter out null or empty values
                .Select(vv => new { value = vv.Value, text = vv.Value }) // Create anonymous objects with both value and text fields
                .ToList();  // Convert to list client-side

            return colorValues;
        }

        public IEnumerable<string> GetColorsForProduct(int productId)
        {
            // First, get all the product variables from the database
            var productVariables = _productsBusiness.GetAll().Where(c => c.Id == productId).SelectMany(c => c.ProductVariables).ToList();  // ToList() forces the data to be retrieved from the database

            // Now, filter variables where the name matches the required color-related values in memory (client-side)
            var colorVariables = productVariables
                .Where(pv => pv.Name == "اللون" || pv.Name == "الالوان" || pv.Name == "لون" || pv.Name == "الوان" || pv.Name == "الألوان" || pv.Name == "ألوان")
                .ToList();  // Apply the filter client-side

            // Extract unique variable values for these filtered variables
            var colorValues = colorVariables
                .SelectMany(pv => pv.VariableValues) // Flatten the variable values
                .DistinctBy(vv => vv.Value) // Ensure uniqueness based on the 'Value' field
                .Where(vv => !string.IsNullOrEmpty(vv.Value)) // Filter out null or empty values
                .Select(vv => vv.Value) // Create anonymous objects with both value and text fields
                .ToList();  // Convert to list client-side

            return colorValues;
        }


        //public IActionResult GetFilteredProducts(int categoryId)
        //{
        //    var filteredProducts = _productsBusiness.GetAll();
        //    if (categoryId > 0)
        //        filteredProducts = filteredProducts.Where(c => c.Categories.Any(cat => cat.CategoryId == categoryId));

        //    filteredProducts.OrderBy(c => c.RegDate).ToList();

        //    // Return the partial view with the filtered products
        //    return PartialView("_FilteredProductsPartial", filteredProducts);
        //}

        //public IActionResult GetFilteredProducts(int categoryId, string selectedSize, int materialId, string color,
        //    int productTypeId, decimal priceFrom, decimal priceTo, int brandId, int manufacturingId)
        //{
        //    // Start by fetching all products
        //    var filteredProducts = _productsBusiness.GetAll();

        //    // Filter by category if categoryId is greater than 0
        //    if (categoryId > 0)
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.Categories.Any(cat => cat.CategoryId == categoryId));
        //    }

        //    // Filter by selected size if it's provided
        //    //if (!string.IsNullOrEmpty(selectedSize))
        //    //{
        //    //    filteredProducts = filteredProducts.Where(c => c.Size == selectedSize);
        //    //}
        //    if (!string.IsNullOrEmpty(selectedSize))
        //    {
        //        // Convert selectedSize to an integer if possible and filter by the size
        //        if (int.TryParse(selectedSize, out int size))
        //        {
        //            filteredProducts = filteredProducts.Where(c => c.ProductVariables
        //                .Any(pv => (pv.Name == "المقاس" || pv.Name == "المقاسات" || pv.Name == "مقاس" || pv.Name == "مقاسات")
        //                           && pv.VariableValues.Any(vv => vv.Value == size.ToString())));
        //        }
        //    }
        //    //// Filter by materialId if it's provided (non-zero)
        //    if (materialId > 0)
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.MaterialId == materialId);
        //    }

        //    //// Filter by productTypeId if it's provided (non-zero)
        //    if (productTypeId > 0)
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.ProductTypeId == productTypeId);
        //    }

        //    //// Set default values for priceFrom and priceTo if they are null
        //    //if (!priceFrom.HasValue) priceFrom = 0; // Default priceFrom to 0
        //    //if (!priceTo.HasValue) priceTo = decimal.MaxValue; // Default priceTo to a very high value

        //    if (priceFrom >= 0 && priceTo != 0)
        //        // Filter by price range if both are provided
        //        filteredProducts = filteredProducts.Where(c => c.DiscountPrice >= priceFrom && c.DiscountPrice <= priceTo);

        //    //// Filter by color name if selectedColorName is provided
        //    if (!string.IsNullOrEmpty(color) && color != "0" && color != "")
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.ProductVariables
        //            .Any(pv => (pv.Name == "اللون" || pv.Name == "الالوان" || pv.Name == "لون" || pv.Name == "الألوان" || pv.Name == "ألوان" || pv.Name == "الوان")
        //                       && pv.VariableValues.Any(vv => vv.Value == color)));
        //    }

        //    if (brandId > 0)
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.BrandId == brandId);
        //    }

        //    if (manufacturingId > 0)
        //    {
        //        filteredProducts = filteredProducts.Where(c => c.ManufacturingId == manufacturingId);
        //    }

        //    // Order by registration date
        //    filteredProducts = filteredProducts.OrderBy(c => c.RegDate);

        //    // Return the filtered products as a partial view
        //    return PartialView("_FilteredProductsPartial", filteredProducts?.ToList());
        //}

        public IActionResult GetFilteredProducts(int categoryId, string selectedSize, string materialId, string color,
    string productTypeId, decimal priceFrom, decimal priceTo, string brandId, string manufacturingId)
        {
            // Start by fetching all products
            var filteredProducts = _productsBusiness.GetAll();

            // Filter by category if categoryId is greater than 0
            if (categoryId > 0)
            {
                filteredProducts = filteredProducts.Where(c => c.Categories.Any(cat => cat.CategoryId == categoryId));
            }

            // Handle selected size
            if (!string.IsNullOrEmpty(selectedSize))
            {
                if (int.TryParse(selectedSize, out int size))
                {
                    filteredProducts = filteredProducts.Where(c => c.ProductVariables
                        .Any(pv => (pv.Name == "المقاس" || pv.Name == "المقاسات" || pv.Name == "مقاس" || pv.Name == "مقاسات")
                                   && pv.VariableValues.Any(vv => vv.Value == size.ToString())));
                }
            }

            // Filter by multiple materials if materialIds are provided
            if (!string.IsNullOrEmpty(materialId))
            {
                var materialIds = materialId.Split(',');  // Split the comma-separated materials
                                                          // Exclude "0" from material IDs filter
                if (materialIds.Contains("0"))
                {
                    materialIds = materialIds.Where(id => id != "0").ToArray();
                }
                if (materialIds.Length > 0)
                {
                    filteredProducts = filteredProducts.Where(c => materialIds.Contains(c.MaterialId.ToString()));
                }
            }

            // Filter by multiple colors if colors are provided
            if (!string.IsNullOrEmpty(color))
            {
                var colors = color.Split(',');  // Split the comma-separated colors
                                                // Exclude "0" from colors filter
                if (colors.Contains("0"))
                {
                    colors = colors.Where(c => c != "0").ToArray();
                }
                if (colors.Length > 0)
                {
                    filteredProducts = filteredProducts.Where(c => c.ProductVariables
                        .Any(pv => (pv.Name == "اللون" || pv.Name == "الالوان" || pv.Name == "لون" || pv.Name == "الألوان" || pv.Name == "ألوان" || pv.Name == "الوان")
                                   && pv.VariableValues.Any(vv => colors.Contains(vv.Value))));
                }
            }

            // Filter by multiple product types if productTypeIds are provided
            if (!string.IsNullOrEmpty(productTypeId))
            {
                var productTypeIds = productTypeId.Split(',');  // Split the comma-separated product types
                                                                // Exclude "0" from product types filter
                if (productTypeIds.Contains("0"))
                {
                    productTypeIds = productTypeIds.Where(id => id != "0").ToArray();
                }
                if (productTypeIds.Length > 0)
                {
                    filteredProducts = filteredProducts.Where(c => productTypeIds.Contains(c.ProductTypeId.ToString()));
                }
            }

            // Filter by price range if both priceFrom and priceTo are provided
            if (priceFrom >= 0 && priceTo != 0)
            {
                filteredProducts = filteredProducts.Where(c => c.DiscountPrice >= priceFrom && c.DiscountPrice <= priceTo);
            }

            // Filter by multiple brands if brandIds are provided
            if (!string.IsNullOrEmpty(brandId))
            {
                var brandIds = brandId.Split(',');  // Split the comma-separated brands
                                                    // Exclude "0" from brand filter
                if (brandIds.Contains("0"))
                {
                    brandIds = brandIds.Where(id => id != "0").ToArray();
                }
                if (brandIds.Length > 0)
                {
                    filteredProducts = filteredProducts.Where(c => brandIds.Contains(c.BrandId.ToString()));
                }
            }

            // Filter by multiple manufacturing IDs if manufacturingIds are provided
            if (!string.IsNullOrEmpty(manufacturingId))
            {
                var manufacturingIds = manufacturingId.Split(',');  // Split the comma-separated manufacturing IDs
                                                                    // Exclude "0" from manufacturing filter
                if (manufacturingIds.Contains("0"))
                {
                    manufacturingIds = manufacturingIds.Where(id => id != "0").ToArray();
                }
                if (manufacturingIds.Length > 0)
                {
                    filteredProducts = filteredProducts.Where(c => manufacturingIds.Contains(c.ManufacturingId.ToString()));
                }
            }

            // Order by registration date
            filteredProducts = filteredProducts.OrderBy(c => c.RegDate);

            // Return the filtered products as a partial view
            return PartialView("_FilteredProductsPartial", filteredProducts?.ToList());
        }


        public IActionResult ViewProduct(int id)
        {
            var userId = GetUserId();
            var reviews = _reviewsBusiness.GetAll();
            ViewBag.Governments = setSelectList(_governmentsBusiness.GetAll().ToList(), "Id", "Name");

            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                ViewBag.UserPhoneNumber = user?.PhoneNumber;
                ViewBag.UserFirstName = user?.FirstName;
                ViewBag.UserLastName = user?.LastName;
            }


            //visitorCount++;

            // Pass the visitor count to the view
            //ViewData["VisitorCount"] = visitorCount;
            var allproducts = _productsBusiness.GetAll();
            ProductDTO product = new ProductDTO();
            product = allproducts.Where(c => c.Id == id).FirstOrDefault();
            var productCategoryIds = product.Categories.Select(c => c.CategoryId).ToList();

            if (User.Identity.IsAuthenticated)
            {
                product.UserAddressDTOs = _userAddressesBusiness.GetUserAddresses(userId);
                product.UserPaymentMethodDTOs = _userPaymentMethodsBusiness.GetUserPaymentMethods(userId);
            }

            if (product != null)
            {
                if (product.ProductImages.Any())
                {
                    product.ProductImages = product.ProductImages.OrderBy(c => c.DisplayOrder).ToList();
                }

                //Get Random Reviews
                product.ProductReviews = reviews
    .OrderBy(r => Guid.NewGuid())  // Randomize the order of reviews
    .Take(4)                      // Take only the first 4 reviews (or fewer if there aren't 4)
    .ToList();                    // Convert the result to a list


                product.HighlyRated = allproducts.OrderBy(c => c.RegDate).Take(4);
                product.ViewRelatedProducts = allproducts
                .Where(p => p.Categories.Any(pc => productCategoryIds.Contains(pc.CategoryId)))
                .OrderBy(p => p.RegDate)
                .Take(4)
                .ToList();
                product.Colors = GetColorsForProduct(id);
                product.Sizes = GetSizesForProduct(id);

                if (product.Duration > 0)
                {
                    DateTime currentDate = DateTime.Now;

                    // Calculate the time difference between the current date and the product's RegDate
                    TimeSpan timeDifference = currentDate - product.RegDate.Value;

                    // Calculate the remaining duration in minutes
                    // Convert the total minutes to an integer (rounding down)
                    int remainingDuration = product.Duration.Value - (int)timeDifference.TotalMinutes;

                    // If the remaining duration is negative, set it to 0
                    remainingDuration = remainingDuration < 0 ? 0 : remainingDuration;

                    // Store the remaining duration or use it as needed
                    product.Duration = remainingDuration;

                }
                else
                {
                    product.Duration = 0;
                }
            }
            return View(product);
        }
        
        public IActionResult BuyNow(int id)
        {
            var userId = GetUserId();
            var reviews = _reviewsBusiness.GetAll();
            ViewBag.Governments = setSelectList(_governmentsBusiness.GetAll().ToList(), "Id", "Name");

            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                ViewBag.UserPhoneNumber = user?.PhoneNumber;
                ViewBag.UserFirstName = user?.FirstName;
                ViewBag.UserLastName = user?.LastName;
            }


            //visitorCount++;

            // Pass the visitor count to the view
            //ViewData["VisitorCount"] = visitorCount;
            var allproducts = _productsBusiness.GetAll();
            ProductDTO product = new ProductDTO();
            product = allproducts.Where(c => c.Id == id).FirstOrDefault();
            var productCategoryIds = product.Categories.Select(c => c.CategoryId).ToList();

            if (User.Identity.IsAuthenticated)
            {
                product.UserAddressDTOs = _userAddressesBusiness.GetUserAddresses(userId);
                product.UserPaymentMethodDTOs = _userPaymentMethodsBusiness.GetUserPaymentMethods(userId);
            }

            if (product != null)
            {
                if (product.ProductImages.Any())
                {
                    product.ProductImages = product.ProductImages.OrderBy(c => c.DisplayOrder).ToList();
                }

                //Get Random Reviews
                product.ProductReviews = reviews
    .OrderBy(r => Guid.NewGuid())  // Randomize the order of reviews
    .Take(4)                      // Take only the first 4 reviews (or fewer if there aren't 4)
    .ToList();                    // Convert the result to a list


                product.Colors = GetColorsForProduct(id);
                product.Sizes = GetSizesForProduct(id);
            }
            return View(product);
        }

        public IActionResult GetRegions(int governmentId)
        {
            var regions = _regionsBusiness.GetAll().Where(c => c.GovernmentId == governmentId).Select(region => new
            {
                id = region.Id,
                text = region.Name
            }).ToList();

            return Json(new { regions = regions });
        }

        public JsonResult GetAddressDetails(int id)
        {
            try
            {
                // Get the address details from the database or other sources
                var addressDetails = _userAddressesBusiness.GetById(id); // Example service call

                // Assuming the addressDetails contain the address, governmentId, regions, and regionId
                var response = new
                {
                    address = addressDetails.Address,    // The full address
                    governmentId = addressDetails.GovernmentId, // The government ID
                    regions = _regionsBusiness.GetAll().Where(c => c.GovernmentId == addressDetails.GovernmentId).Select(r => new { r.Id, r.Name }).ToList(), // List of regions
                    regionId = addressDetails.RegionId  // The regionId to set as selected
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public void populateViewBags()
        {
            ViewBag.Materials = setSelectList(_materialsBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.ProductTypes = setSelectList(_productTypesBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Colors = setSelectList(GetColors(), "value", "text");
            ViewBag.Brands = setSelectList(_brandsBusiness.GetAll().ToList(), "Id", "Name");
            ViewBag.Manufacturing = setSelectList(_manufacturingBusiness.GetAll().ToList(), "Id", "Name");
        }

        [HttpPost]
        public JsonResult CheckAvailability(int quantity, string color, string size, int productId)
        {
            // Check the availability based on the received data
            int combinationId = CheckProductAvailability(quantity, color, size, productId);

            if (combinationId > 0)
                return Json(combinationId);
            else
                return Json(false);
        }

        [HttpPost]
        public JsonResult CheckPieceAvailability(string[] text, int productId)
        {
            // Check the availability based on the received data
            int combinationId = CheckProductPieceAvailability(text, productId);

            if (combinationId > 0)
                return Json(combinationId);
            else
                return Json(false);
        }


        //private int CheckProductAvailability(int quantity, string color, string size, int productId)
        //{
        //    var product = _productsBusiness.GetAll().Where(c => c.Id == productId).FirstOrDefault();
        //    if (product != null && product.VariableCombinations != null)
        //    {
        //        var combination = product.VariableCombinations.FirstOrDefault(c => c.Text.Contains(color) && c.Text.Contains(size));

        //        if (combination != null)
        //        {
        //            if (quantity <= combination.StockCount)
        //            {
        //                return (int)combination.Id; // Product is available
        //            }
        //        }
        //    }


        //    return 0; // Product is not available
        //}

        private int CheckProductAvailability(int quantity, string color, string size, int productId)
        {
            var product = _productsBusiness.GetAll().Where(c => c.Id == productId).FirstOrDefault();

            if (product != null && product.VariableCombinations != null)
            {
                // If both color and size are provided, check for combination with both
                if (!string.IsNullOrEmpty(color) && !string.IsNullOrEmpty(size))
                {
                    var combination = product.VariableCombinations.FirstOrDefault(c => c.Text.Contains(color) && c.Text.Contains(size));

                    if (combination != null && quantity <= combination.StockCount)
                    {
                        return (int)combination.Id; // Product is available
                    }
                }
                // If only color is provided, check for combination with color
                else if (!string.IsNullOrEmpty(color) && string.IsNullOrEmpty(size))
                {
                    var combination = product.VariableCombinations.FirstOrDefault(c => c.Text.Contains(color));

                    if (combination != null && quantity <= combination.StockCount)
                    {
                        return (int)combination.Id; // Product is available
                    }
                }
                // If only size is provided, check for combination with size
                else if (string.IsNullOrEmpty(color) && !string.IsNullOrEmpty(size))
                {
                    var combination = product.VariableCombinations.FirstOrDefault(c => c.Text.Contains(size));

                    if (combination != null && quantity <= combination.StockCount)
                    {
                        return (int)combination.Id; // Product is available
                    }
                }
            }

            return 0; // Product is not available
        }


        private int CheckProductPieceAvailability(string[] text, int productId)
        {
            var product = _productsBusiness.GetAll().FirstOrDefault(c => c.Id == productId);

            if (product != null && product.VariableCombinations != null)
            {
                // Assuming the text array contains values for color and size (in order)
                var color = text[0];  //colorOrSize
                var size = text[1];   //colorOrSize

                var combination = product.VariableCombinations.FirstOrDefault(c => c.Text.Contains(color) && c.Text.Contains(size));

                if (combination != null)
                {
                    if (combination.StockCount >= 1)
                    {
                        return (int)combination.Id; // Product is available
                    }
                }
            }

            return 0; // Product is not available
        }



        [HttpPost]
        //[Authorize]
        public JsonResult AddToCart(int quantity, int combinationId, int productId)
        {
            var result = AddProductToCart(quantity, combinationId, productId);

            if (result)
            {
                return Json(true); // Return true if successfully added to cart
            }
            else
            {
                return Json(false); // Return false if an error occurred
            }
        }

        [Authorize]
        private bool AddProductToCart(int quantity, int combinationId, int productId)
        {
            var userId = GetUserId();
            var sessionId = GetGuestSessionId();

            ShoppingCartDTO cart = new ShoppingCartDTO
            {
                ProductId = productId,
                CombinationId = combinationId,
                Qty = quantity,
            };

            if (!string.IsNullOrEmpty(userId))
                cart.UserId = userId;
            else
                cart.GuestSessionId = sessionId;

            var userShoppingCart = _shoppingCartsBusiness.GetByUserId(userId);
            if (userShoppingCart != null && userShoppingCart.Count() > 0)
            {
                var sameProductWithoutOffer = userShoppingCart.FirstOrDefault(c => c.offerId == null && c.ProductId == productId);
                if (sameProductWithoutOffer != null)
                {
                    cart.Id = sameProductWithoutOffer.Id;
                    cart.Qty += sameProductWithoutOffer.Qty;
                }
            }

            var result = _shoppingCartsBusiness.AddUpdate(cart);

            if (result == (int)SignatureResponseMessage.SuccessInsert || result == (int)SignatureResponseMessage.SuccessUpdate)
                return true;

            return false;
        }


        [HttpPost]
        //[Authorize]
        public JsonResult AddOfferToCart(int[] combinationIds, int productId, int offerId)
        {
            // You can retrieve the userId as needed
            var userId = GetUserId();
            var sessionId = GetGuestSessionId();

            List<ShoppingCartDTO> cart = new List<ShoppingCartDTO>();

            // Example: Iterate through the combinationIds and add them to the cart
            bool isSuccess = false;

            var regDate = DateTime.Now;

            foreach (var combinationId in combinationIds)
            {
                var cartItem = new ShoppingCartDTO
                {
                    ProductId = productId,
                    CombinationId = combinationId,
                    Qty = 1,
                    RegDate = regDate,
                    offerId = offerId
                };

                if (!string.IsNullOrEmpty(userId))
                    cartItem.UserId = userId;
                else
                    cartItem.GuestSessionId = sessionId;

                cart.Add(cartItem);
            }

            if (cart.Count > 0)
            {
                var result = _shoppingCartsBusiness.AddUpdateRange(cart);
                if (result == (int)SignatureResponseMessage.SuccessInsert || result == (int)SignatureResponseMessage.SuccessUpdate)
                    isSuccess = true;
            }

            if (isSuccess)
            {
                return Json(true); // Return true if the offer was successfully added to the cart
            }
            else
            {
                return Json(false); // Return false if there was an error
            }
        }

        [HttpPost]
        public async Task<IActionResult>? PlaceItemOrder(
        int paymentMethodId, int governmentId, int? regionId, string notes, string firstName, string lastName, string phoneNumber, string phoneNumber2, string deliveryTimeFrom, string deliveryTimeTo,
        decimal totalValue, decimal discountValue, decimal netAmountValue, string newGovernmentValue, string newRegionValue, string address, string passwordValue, int quantity,
        int combinationId, int productId)
        {
            // Assuming you have a service method to process the order
            try
            {
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(address) || paymentMethodId <= 0 || governmentId <= 0)
                {
                    return Json(new { success = false, message = "Please complete all required fields." });
                }
                var userId = GetUserId();
                var sessionId = GetGuestSessionId();

                if (string.IsNullOrEmpty(userId))
                {
                    //try to get user with same phone number
                    var existinguser = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == phoneNumber);
                    if (existinguser != null)
                    {
                        userId = existinguser.Id;
                    }
                }



                //Check if new area

                if (governmentId == -1)
                {
                    //Add Region And Government
                    GovernmentDTO existionGovernment = _governmentsBusiness.GetByName(newGovernmentValue);
                    if (existionGovernment != null)
                        governmentId = (int)existionGovernment.Id;
                    else
                    {
                        //Add the government and get it;
                        GovernmentDTO newGovernment = new GovernmentDTO { Name = newGovernmentValue };
                        var addResult = _governmentsBusiness.AddUpdate(newGovernment);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            governmentId = (int)_governmentsBusiness.GetByName(newGovernmentValue).Id;
                        }
                    }
                }
                if (regionId == -1)
                {
                    //Add Region And Government
                    RegionDTO existionRegion = _regionsBusiness.GetByName(newRegionValue);
                    if (existionRegion != null)
                        regionId = (int)existionRegion.Id;
                    else
                    {
                        //Add the government and get it;
                        RegionDTO newRegion = new RegionDTO { Name = newRegionValue };
                        var addResult = _regionsBusiness.AddUpdate(newRegion);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            regionId = (int)_regionsBusiness.GetByName(newRegionValue).Id;
                        }
                    }
                }


                if (string.IsNullOrEmpty(userId))
                {
                    //Register new user
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        UserName = phoneNumber,
                        Address = address,
                        RegionId = regionId,
                        GovernmentId = governmentId,
                        AutoRegistered = true
                    };

                    var result2 = await _userManager.CreateAsync(user, passwordValue);

                    if (result2.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(user, new string[] { Roles.Role_Customer });

                        //Add User Address
                        var userAddressDTO = new UserAddressDTO
                        {
                            UserId = user.Id,
                            Name = "المنزل",
                            GovernmentId = governmentId,
                            RegionId = regionId,
                            Address = address,
                            AddressTypeId = 1 //المنزل
                        };
                        _userAddressesBusiness.AddUpdate(userAddressDTO);

                        //Add User Payment Method
                        var userPaymentMethod = new UserPaymentMethodDTO
                        {
                            userId = user.Id,
                            isDefault = true,
                            PaymentMethodId = 1 //كاش

                        };
                        _userPaymentMethodsBusiness.AddUpdate(userPaymentMethod);
                    }
                    else
                    {
                        return Json(new { success = false, message = "error creating user" });
                    }

                }
                var regDate = DateTime.Now;
                List<OrderDetailsDTO> orderDetailsDTOs = new List<OrderDetailsDTO>();
                var orderDetail = new OrderDetailsDTO
                {
                    RegDate = regDate,
                    UserId = userId,
                    ProductId = productId,
                    CombinationId = combinationId,
                    Qty = quantity

                };
                //if (combinationId != null && combinationId > 0)
                //    updateStock(orderDetail.ProductId, (int)orderDetail.CombinationId, orderDetail.Qty);
                
                orderDetailsDTOs.Add(orderDetail);

                var orderHead = new OrderHeadDTO
                {
                    OrderDetails = orderDetailsDTOs,
                    UserId = userId,
                    RegDate = regDate,
                    Total = totalValue,
                    Discount = discountValue,
                    NetAmount = netAmountValue,
                    orderNo = regDate.ToString("yyMMddHHss").Substring(2, 6) + new Random().Next(0, 99).ToString("D2"),
                    Notes = notes,
                    fullName = firstName + " " + lastName,
                    paymentMethodId = paymentMethodId,
                    governmentId = governmentId,
                    regionId = regionId,
                    deliveryTimeFrom = deliveryTimeFrom,
                    deliveryTimeTo = deliveryTimeTo,
                    Address = address,
                    phoneNumber = phoneNumber,
                    phoneNumber2 = phoneNumber2

                };

                var result = _ordersBusiness.AddUpdate(orderHead);

                if (result == (int)SignatureResponseMessage.SuccessInsert)
                {
                    //get last inserted order for user
                    var insertedorder = _ordersBusiness.GetAll().Where(c => c.UserId == userId).OrderBy(c=>c.Id).LastOrDefault();

                    AppendOrderToGoogleSheet(insertedorder);
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult>? PlaceOfferOrder(int paymentMethodId, int governmentId, int? regionId, string notes, string firstName, string lastName, string phoneNumber, string phoneNumber2, string deliveryTimeFrom, string deliveryTimeTo,
        decimal totalValue, decimal discountValue, decimal netAmountValue, string newGovernmentValue, string newRegionValue, string address, string passwordValue, int productId, int[] combinationIds, int offerId)
        {
            try
            {
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber) || paymentMethodId <= 0 || governmentId <= 0)
                {
                    return Json(new { success = false, message = "Please complete all required fields." });
                }
                var userId = GetUserId();
                var sessionId = GetGuestSessionId();

                if (string.IsNullOrEmpty(userId))
                {
                    //try to get user with same phone number
                    var existinguser = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == phoneNumber);
                    if (existinguser != null)
                    {
                        userId = existinguser.Id;
                    }
                }



                //Check if new area

                if (governmentId == -1)
                {
                    //Add Region And Government
                    GovernmentDTO existionGovernment = _governmentsBusiness.GetByName(newGovernmentValue);
                    if (existionGovernment != null)
                        governmentId = (int)existionGovernment.Id;
                    else
                    {
                        //Add the government and get it;
                        GovernmentDTO newGovernment = new GovernmentDTO { Name = newGovernmentValue };
                        var addResult = _governmentsBusiness.AddUpdate(newGovernment);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            governmentId = (int)_governmentsBusiness.GetByName(newGovernmentValue).Id;
                        }
                    }
                }
                if (regionId == -1)
                {
                    //Add Region And Government
                    RegionDTO existionRegion = _regionsBusiness.GetByName(newRegionValue);
                    if (existionRegion != null)
                        regionId = (int)existionRegion.Id;
                    else
                    {
                        //Add the government and get it;
                        RegionDTO newRegion = new RegionDTO { Name = newRegionValue };
                        var addResult = _regionsBusiness.AddUpdate(newRegion);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            regionId = (int)_regionsBusiness.GetByName(newRegionValue).Id;
                        }
                    }
                }

                if (string.IsNullOrEmpty(userId))
                {
                    //Register new user
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        UserName = phoneNumber,
                        Address = address,
                        RegionId = regionId,
                        GovernmentId = governmentId,
                        AutoRegistered = true
                    };

                    var result2 = await _userManager.CreateAsync(user, passwordValue);

                    if (result2.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(user, new string[] { Roles.Role_Customer });

                        //Add User Address
                        var userAddressDTO = new UserAddressDTO
                        {
                            UserId = user.Id,
                            Name = "المنزل",
                            GovernmentId = governmentId,
                            RegionId = regionId,
                            Address = address,
                            AddressTypeId = 1 //المنزل
                        };
                        _userAddressesBusiness.AddUpdate(userAddressDTO);

                        //Add User Payment Method
                        var userPaymentMethod = new UserPaymentMethodDTO
                        {
                            userId = user.Id,
                            isDefault = true,
                            PaymentMethodId = 1 //كاش

                        };
                        _userPaymentMethodsBusiness.AddUpdate(userPaymentMethod);
                    }
                    else
                    {
                        return Json(new { success = false, message = "error creating user" });
                    }
                }



                var regDate = DateTime.Now;
                List<OrderDetailsDTO> orderDetailsDTOs = new List<OrderDetailsDTO>();

                foreach (var combinationId in combinationIds)
                {
                    var orderDetail = new OrderDetailsDTO
                    {
                        RegDate = regDate,
                        UserId = userId,
                        ProductId = productId,
                        CombinationId = combinationId,
                        Qty = 1,
                        offerId = offerId

                    };
                    //if (combinationId != null && combinationId > 0)
                    //    updateStock(orderDetail.ProductId, (int)orderDetail.CombinationId, orderDetail.Qty);

                    orderDetailsDTOs.Add(orderDetail);

                }
                var orderHead = new OrderHeadDTO
                {
                    OrderDetails = orderDetailsDTOs,
                    UserId = userId,
                    RegDate = regDate,
                    Total = totalValue,
                    Discount = discountValue,
                    NetAmount = netAmountValue,
                    orderNo = regDate.ToString("yyMMddHHss").Substring(2, 6) + new Random().Next(0, 99).ToString("D2"),
                    Notes = notes,
                    fullName = firstName + " " + lastName,
                    paymentMethodId = paymentMethodId,
                    governmentId = governmentId,
                    regionId = regionId,
                    deliveryTimeFrom = deliveryTimeFrom,
                    deliveryTimeTo = deliveryTimeTo,
                    Address = address,
                    phoneNumber = phoneNumber,
                    phoneNumber2 = phoneNumber2

                };

                var result = _ordersBusiness.AddUpdate(orderHead);

                if (result == (int)SignatureResponseMessage.SuccessInsert)
                {
                    var insertedorder = _ordersBusiness.GetAll().Where(c => c.UserId == userId).OrderBy(c => c.Id).LastOrDefault();

                    AppendOrderToGoogleSheet(insertedorder);
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false });
            }
        }



        public async Task<IActionResult> AppendOrderToGoogleSheet(OrderHeadDTO order)
        {
            if (order == null)
            {
                return NotFound();  // If the order doesn't exist
            }

            // Step 2: Prepare the order data
            var orderData = new List<IList<object>>()
            {
                // Add headers for the order (optional)
                //new List<object> { "اسم العميل", "تاريخ الاوردر", "رقم الاوردر", "طريقة الدفع", "المحافظة", "المنطقة", "العنوان", "الصنف", "الكمية", "السعر", "الاجمالي" }
            };

            // Prepare the first row (main order details)
            var orderRow = new List<object>
    {
        order?.RegDate.ToString("yyyy-MM-dd"),  // Order Date
        order?.orderNo,  // Order No
        order?.fullName,  // Customer Name
        order?.phoneNumber?.Substring(1),  // Phone number 1
        order?.phoneNumber2?.Substring(1),  // Phone number 2
        order?.Government?.Name,  // Government
        order?.Region?.Name,  // Region
        order?.Address,  // Address
        order.deliveryTimeFrom,  // Delivery Time From
        order.deliveryTimeTo,  // Delivery Time To
        order.Notes,  // Notes
    };

            // Prepare placeholders for each product's details (e.g., item names, quantities, prices, etc.)
            var itemNames = new StringBuilder();
            var quantities = new StringBuilder();
            var prices = new StringBuilder();
            var totals = new StringBuilder();

            // If there are any order details (items), loop through them
            if (order?.OrderDetails != null && order.OrderDetails.Any())
            {
                foreach (var detail in order.OrderDetails)
                {
                    // Add item details to the corresponding StringBuilder
                    for (int i = 0; i < detail?.Qty; i++)  // Loop for each quantity
                    {
                        if (itemNames.Length > 0) itemNames.AppendLine();  // Add line break before appending the next item
                        itemNames.Append(detail?.Product?.ArbName + " - " + FormatProductDetails(detail?.Combination?.Text));  // Item name

                        if (quantities.Length > 0) quantities.AppendLine();  // Add line break before appending the next quantity
                        quantities.Append(1);  // Always append 1 for quantity

                        if (prices.Length > 0) prices.AppendLine();  // Add line break before appending the next price
                        prices.Append((int)detail?.Product?.DiscountPrice);  // Price

                        if (totals.Length > 0) totals.AppendLine();  // Add line break before appending the next total
                        totals.Append((int)detail?.Product?.DiscountPrice);  // Total (always price for each row)
                    }
                }
                // Add the item details to the order row, starting from column 11 (index 10)
                orderRow.Add(itemNames.ToString());  // Item names in the same cell, each on a new line
                orderRow.Add(quantities.ToString());  // Quantities in the same cell, each on a new line
                orderRow.Add(prices.ToString());      // Prices in the same cell, each on a new line
                orderRow.Add(totals.ToString());      // Totals in the same cell, each on a new line
            }
            else
            {
                // If there are no items, add empty placeholders for items, quantities, prices, and totals
                orderRow.Add("No items");  // Add "No items" message to the row
                orderRow.Add("");  // Empty cell for quantities
                orderRow.Add("");  // Empty cell for prices
                orderRow.Add("");  // Empty cell for totals
            }

            orderRow.Add(order?.NetAmount);
            // Add the first order row to the orderData list
            orderData.Add(orderRow);

            // Step 4: Call your business logic to append this data to Google Sheets
            var result = await _googleSheetBusiness.AppendDataAsync(orderData);

            if (result)
            {
                return RedirectToAction("Success");  // Redirect to a success page
            }

            return View("Error");  // If there was an error
        }






        //public void updateStock(int productId, int combinationId, int qty)
        //{
        //    var product = _productsBusiness.GetById(productId);
        //    if (product != null && product.TrackStock)
        //    {
        //        var combination = product?.VariableCombinations?.FirstOrDefault(c => c.Id == combinationId);
        //        if (combination != null)
        //        {
        //            // Check if there is enough stock before decrementing
        //            if (combination.StockCount >= qty)
        //            {
        //                combination.StockCount -= qty;
        //                _productsBusiness.AddUpdate(product);
        //            }
        //        }

        //    }
        //}

    }
}

