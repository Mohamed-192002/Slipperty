using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Business.DTO;
using Infrastructure.Consts;
using Infrastructure.Contracts.Seeds;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static Business.Helpers.BusinessHelpers;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    //[Authorize]
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartsBusiness _ShoppingCartsBusiness;
        private readonly IUserAddressesBusiness _userAddressesBusiness;
        private readonly IUserPaymentMethodsBusiness _userPaymentMethodsBusiness;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGovernmentsBusiness _governmentsBusiness;
        private readonly IRegionsBusiness _regionsBusiness;
        private readonly IOrdersBusiness _ordersBusiness;
        private readonly IGoogleSheetBusiness _googleSheetBusiness;
        private readonly IProductsBusiness _productsBusiness;
        private readonly INotifyOrderTransactionService _notifyOrderTransactionService;


        public ShoppingCartController(IShoppingCartsBusiness ShoppingCartsBusiness, IUserAddressesBusiness userAddressesBusiness, IUserPaymentMethodsBusiness userPaymentMethodsBusiness,
            ICategoriesBusiness categoriesBusiness, IGovernmentsBusiness governmentsBusiness, IRegionsBusiness regionsBusiness, IOrdersBusiness ordersBusiness, IPixelSettingsBusiness pixelSettingsBusiness,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IGoogleSheetBusiness googleSheetBusiness, IProductsBusiness productsBusiness, INotifyOrderTransactionService notifyOrderTransactionService)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _ShoppingCartsBusiness = ShoppingCartsBusiness;
            _userAddressesBusiness = userAddressesBusiness;
            _userPaymentMethodsBusiness = userPaymentMethodsBusiness;
            _governmentsBusiness = governmentsBusiness;
            _userManager = userManager;
            _regionsBusiness = regionsBusiness;
            _ordersBusiness = ordersBusiness;
            _googleSheetBusiness = googleSheetBusiness;
            _productsBusiness = productsBusiness;
            _notifyOrderTransactionService = notifyOrderTransactionService;
        }

        //[Authorize]
        public IActionResult Index()
        {
            List<ShoppingCartDTO> DataList = new List<ShoppingCartDTO>();
            var userId = GetUserId();
            var sessionId = GetGuestSessionId();

            if (!string.IsNullOrEmpty(userId))
            {
                DataList = _ShoppingCartsBusiness.GetByUserId(userId).OrderBy(u => u.Id).ToList();
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                ViewBag.UserPhoneNumber = user?.PhoneNumber;
                ViewBag.UserFirstName = user?.FirstName;
                ViewBag.UserLastName = user?.LastName;
            }
            else
            {
                DataList = _ShoppingCartsBusiness.GetBySessionId(sessionId).OrderBy(u => u.Id).ToList();
            }

            ShoppingCartVM ShippingCartVM = new ShoppingCartVM();



            ShippingCartVM.shoppingCartDTOs = DataList;
            ShippingCartVM.UserAddressDTOs = _userAddressesBusiness.GetUserAddresses(userId);
            ShippingCartVM.UserPaymentMethodDTOs = _userPaymentMethodsBusiness.GetUserPaymentMethods(userId);


            ViewBag.Governments = setSelectList(_governmentsBusiness.GetAll().ToList(), "Id", "Name");

            return View(ShippingCartVM);
        }

        [HttpPost]
        public JsonResult updateCartItemCount(int cartItemId, int count)
        {
            try
            {
                // Update the cart item count logic here, e.g., update in database
                var cartItem = _ShoppingCartsBusiness.GetById(cartItemId);

                if (cartItem != null)
                {
                    cartItem.Qty = count; // Update the quantity
                    _ShoppingCartsBusiness.AddUpdate(cartItem); // Save the changes

                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult deleteCartItem(int cartItemId)
        {
            try
            {
                if (cartItemId > 0)
                {
                    _ShoppingCartsBusiness.Delete(cartItemId, false);

                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult deleteCartOffer(int offerId, DateTime regDate)
        {
            try
            {
                // Validate the offerId
                if (offerId > 0)
                {
                    // Call your business logic to delete the cart items based on offerId and regDate
                    var isDeleted = _ShoppingCartsBusiness.DeleteOfferFromCart(offerId, regDate);

                    // Return success if the deletion was successful
                    if (isDeleted)
                    {
                        return Json(new { success = true });
                    }
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return Json(new { success = false, message = ex.Message });
            }
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

        public IActionResult GetRegions(int governmentId)
        {
            var regions = _regionsBusiness.GetAll().Where(c => c.GovernmentId == governmentId).Select(region => new
            {
                id = region.Id,
                text = region.Name
            }).ToList();

            return Json(new { regions = regions });
        }


        [HttpPost]
        public async Task<JsonResult> PlaceOrder(int paymentMethodId, int governmentId, int regionId, string notes, string firstName, string lastName, string phoneNumber, string phoneNumber2, string deliveryTimeFrom,
            string deliveryTimeTo, decimal totalValue, decimal netAmountValue, decimal discountValue, string newGovernmentValue, string newRegionValue, string passwordValue, string address)
        {
            try
            {

                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber) || paymentMethodId <= 0 || governmentId <= 0 || regionId <= 0)
                {
                    return Json(new { success = false, message = "Please complete all required fields." });
                }
                var userCart = new List<ShoppingCartDTO>();
                var userId = GetUserId();
                var sessionId = GetGuestSessionId();

                if (string.IsNullOrEmpty(userId))
                {
                 
                    var existinguser = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == phoneNumber);
                    if (existinguser != null)
                    {
                        userId = existinguser.Id;
                    }
                }


                if (!string.IsNullOrEmpty(userId))
                {
                    userCart = _ShoppingCartsBusiness.GetByUserId(userId).ToList();
                }
                else
                {
                    userCart = _ShoppingCartsBusiness.GetByUserId(userId).ToList();

                

                    if (governmentId == -1)
                    {
                
                        GovernmentDTO existionGovernment = _governmentsBusiness.GetByName(newGovernmentValue);
                        if (existionGovernment != null)
                            governmentId = (int)existionGovernment.Id;
                        else
                        {
                
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

                foreach (var item in userCart)
                {
                    var orderDetail = new OrderDetailsDTO
                    {
                        RegDate = regDate,
                        UserId = userId,
                        ProductId = item.ProductId,
                        CombinationId = item.CombinationId,
                        Qty = item.Qty,
                        offerId = item?.offerId

                    };
                    //if (item.CombinationId != null && item.CombinationId > 0)
                    //    updateStock(item.ProductId, item.CombinationId, item.Qty);
                    
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
                    orderNo =     (long.Parse(regDate.ToString("yyddMMss")) + (await _ordersBusiness.GetAll()
                    .CountAsync(o => o.RegDate == regDate.Date) + 1)).ToString(), 
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
                    if (!string.IsNullOrEmpty(sessionId))
                    {

                        _ShoppingCartsBusiness.EmptySessionCart(sessionId);
                    }
                    else
                    {

                        _ShoppingCartsBusiness.EmptyUserCart(userId);
                    }
                }
                _notifyOrderTransactionService.ReceiveNewOrderNotification();
                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
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
