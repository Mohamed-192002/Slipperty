using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Business.Helpers.BusinessHelpers;

namespace InventoryManagement.Controllers
{
    [Area("Home")]
    public class BaseController : Controller
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        private readonly ICategoriesBusiness _categoriesBusiness;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPixelSettingsBusiness _pixelSettingsBusiness;

        public BaseController(ICategoriesBusiness categoriesBusiness, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
        {
            _categoriesBusiness = categoriesBusiness;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _pixelSettingsBusiness = pixelSettingsBusiness;
        }

        public bool hasRelation(string searchWord, string ColNameToSearch)
        {
            bool res = _categoriesBusiness.hasRelation(searchWord, ColNameToSearch);
            return res;
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }


        //public void SetTempMessage(string key, string value)
        //{
        //    TempData[key] = _localizer.GetValue(value).ToString();
        //}

        public void SetTempMessage(string key, string value)
        {
            TempData[key] = value;
        }

        public void setUpsertTempMessages(int result)
        {
            if (result == (int)SignatureResponseMessage.SuccessInsert)
                SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessInsert.ToString());
            else if (result == (int)SignatureResponseMessage.SuccessUpdate)
                SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessUpdate.ToString());
            else
                SetTempMessage(ResponseTypes.Error, ResponseMessages.Error.ToString());
        }

        public void setDeleteMessages(bool result)
        {
            if (result)
                SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessDelete.ToString());
            else
                SetTempMessage(ResponseTypes.Error, ResponseMessages.NotDeleted.ToString());
        }

        public SelectList setSelectList(IEnumerable items, string dataValueField, string ArabicDataTextField)
        {
            return new SelectList(items, dataValueField, ArabicDataTextField);
        }

        //public SelectList setSelectList<T>(IEnumerable<T> items, Func<T, string> dataValueField, Func<T, string> ArabicDataTextField,Func<T, string> EnglishDataTextField)
        //{
        //    // Prepare a list of SelectListItem to pass to SelectList constructor
        //    var selectListItems = items.Select(item => new SelectListItem
        //    {
        //        Value = GetPropertyValue(item, dataValueField(item)), // Use reflection to get the value of dataValueField
        //        Text = CultureInfo.CurrentCulture.Name.StartsWith("ar")
        //               ? ArabicDataTextField(item)  // If the current culture is Arabic, use the Arabic field
        //               : EnglishDataTextField(item)  // Otherwise, use the English field
        //    }).ToList();

        //    return new SelectList(selectListItems, "Value", "Text");
        //}

        private string GetPropertyValue<T>(T item, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(item);
                return value?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        public byte[] GetBytes(IFormFile formFile)
        {
            var memoryStream = new MemoryStream();
            formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return false;

            if (file.Length > MaxFileSize)
                return false;

            return true;
        }

        public string GetUserId()
        {
            // Get the user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return string.Empty;
            }

            return userId;
        }

        public async Task<ApplicationUser> getUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public string getWwwRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }

        public void saveImage(string path, string fileName, IFormFile file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        public void deleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public void SetPagination(int pageSize, int pageNumber, int totalPages, int DisplayedItemsCount)
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.IsPreviousLinkVisible = pageNumber > 1;
            ViewBag.IsNextLinkVisible = pageNumber < totalPages;
            ViewBag.DisplayedItemsCount = DisplayedItemsCount;
        }

        public IActionResult GetCategories()
        {
            IEnumerable<CategoryDTO> categories = _categoriesBusiness.GetAll();
            return PartialView("_categoriesListPartial", categories);
        }

        public string GetGuestSessionId()
        {
            var context = _httpContextAccessor.HttpContext;
            var guestSessionId = context?.Request.Cookies["GuestSessionId"];

            if (string.IsNullOrEmpty(guestSessionId))
            {
                guestSessionId = Guid.NewGuid().ToString();
                context?.Response.Cookies.Append("GuestSessionId", guestSessionId, new CookieOptions { Expires = DateTimeOffset.Now.AddMonths(1) });
            }

            return guestSessionId;
        }

        [HttpGet]
        public IActionResult GetFacebookPixelIds()
        {
            var pixelSettings = _pixelSettingsBusiness.GetAll().FirstOrDefault();

            if (pixelSettings == null || string.IsNullOrEmpty(pixelSettings.facebook))
            {
                return Json(new { PixelIds = new string[] { } }); // Return an empty array if no Pixel IDs are found
            }

            // Split the Pixel IDs by newline characters and return them as an array
            var pixelIds = pixelSettings.facebook.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return Json(new { PixelIds = pixelIds });
        }

        [HttpGet]
        public IActionResult GetTiktokPixelIds()
        {
            var pixelSettings = _pixelSettingsBusiness.GetAll().FirstOrDefault();

            if (pixelSettings == null || string.IsNullOrEmpty(pixelSettings.tiktok))
            {
                return Json(new { PixelIds = new string[] { } }); // Return an empty array if no Pixel IDs are found
            }

            // Split the Pixel IDs by newline characters and return them as an array
            var pixelIds = pixelSettings.tiktok.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return Json(new { PixelIds = pixelIds });
        }
        
        [HttpGet]
        public IActionResult GetclaritylIds()
        {
            var pixelSettings = _pixelSettingsBusiness.GetAll().FirstOrDefault();

            if (pixelSettings == null || string.IsNullOrEmpty(pixelSettings.clarity))
            {
                return Json(new { clarityIds = new string[] { } }); // Return an empty array if no Pixel IDs are found
            }

            // Split the Pixel IDs by newline characters and return them as an array
            var clarityIds = pixelSettings.clarity.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return Json(new { clarityIds = clarityIds });
        }

        public string FormatProductDetails(string text)
        {
            // Define regex pattern to extract all values after ":" and before ","
            var pattern = @"(?:\S+:\s*)([^,]+)"; // Matches "key: value" and captures the value

            // Use regex to find all values
            var matches = Regex.Matches(text, pattern);

            // Extract values and join them with " - "
            var formattedText = string.Join(" - ", matches.Cast<Match>().Select(m => m.Groups[1].Value));

            return formattedText;
        }
    }
}
