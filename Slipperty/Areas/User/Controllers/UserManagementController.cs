using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Business.DTO;
using Infrastructure.Contracts.Seeds;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using static Business.Helpers.BusinessHelpers;

namespace Slipperty.Areas.User.Controllers
{
    [Area("User")]
    public class UserManagementController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGovernmentsBusiness _governmentsBusiness;
        private readonly IRegionsBusiness _regionsBusiness;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWhatsAppAPIBusiness _whatsAppAPIBusiness;
        private readonly IUserAddressesBusiness _userAddressesBusiness;
        private readonly IUserPaymentMethodsBusiness _userPaymentMethodsBusiness;
        private readonly IAddressTypesBusiness _addressTypesBusiness;

        public UserManagementController(IGovernmentsBusiness governmentsBusiness, IRegionsBusiness regionsBusiness, ICategoriesBusiness categoriesBusiness,
            IUserAddressesBusiness userAddressesBusiness, IUserPaymentMethodsBusiness userPaymentMethodsBusiness, IAddressTypesBusiness addressTypesBusiness,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment,
            IWhatsAppAPIBusiness whatsAppAPIBusiness, IHttpContextAccessor httpContextAccessor, IPixelSettingsBusiness pixelSettingsBusiness)
            : base(categoriesBusiness, userManager, webHostEnvironment, httpContextAccessor, pixelSettingsBusiness)
        {
            _governmentsBusiness = governmentsBusiness;
            _regionsBusiness = regionsBusiness;
            _userManager = userManager;
            _whatsAppAPIBusiness = whatsAppAPIBusiness;
            _signInManager = signInManager;
            _userAddressesBusiness = userAddressesBusiness;
            _userPaymentMethodsBusiness = userPaymentMethodsBusiness;
            _addressTypesBusiness = addressTypesBusiness;
        }
        public IActionResult SignUp(string phoneNumber)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.PhoneNumber = phoneNumber;
            populateViewBags();
            return View(userDTO);
        }

        public void populateViewBags()
        {
            ViewBag.Governments = setSelectList(_governmentsBusiness.GetAll().ToList(), "Id", "Name");
        }

        public IActionResult RegisterPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterPhoneNumber(RegisterPhoneNumberDTO registerPhoneNumberDTO)
        {

            if (ModelState.IsValid)
            {
                if (registerPhoneNumberDTO.PhoneNumber.Length == 11)
                    registerPhoneNumberDTO.PhoneNumber = "2" + registerPhoneNumberDTO.PhoneNumber;
                var existingUser = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == registerPhoneNumberDTO.PhoneNumber);
                if (existingUser != null && !existingUser.AutoRegistered)
                {
                    ModelState.AddModelError("PhoneNumber", "هذا الرقم مسجل من قبل");
                    return View(registerPhoneNumberDTO);
                }
                else if(existingUser != null && !existingUser.AutoRegistered)
                {
                    //
                    return RedirectToAction(nameof(SetPassword), new { userId = existingUser.Id });
                }


                return RedirectToAction(nameof(SignUp), new { phoneNumber = registerPhoneNumberDTO.PhoneNumber });
            }
            return View(registerPhoneNumberDTO);
        }

        public async Task<IActionResult> SetPassword(string userId)
        {
            var setPasswordDTO = new SetPasswordDTO
            {
                userId = userId,
            };


            if (!string.IsNullOrEmpty(userId))
            {

                //check if user exists
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return NotFound();

                setPasswordDTO.userId = user.Id;
            }

            return View(setPasswordDTO);
        }

        public async Task<IActionResult> ChangePassword(SetPasswordDTO setPasswordDTO)
        {

            if (!string.IsNullOrEmpty(setPasswordDTO.userId))
            {

                //check if user exists
                var user = await _userManager.FindByIdAsync(setPasswordDTO.userId);

                if (user == null)
                    return NotFound();



                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, setPasswordDTO.Password);

                if (result.Succeeded)
                {
                    user.AutoRegistered = false;
                    await _userManager.UpdateAsync(user);
                    SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessUpdate);

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    SetTempMessage(ResponseTypes.Error, ResponseMessages.Error);
                    return View(setPasswordDTO);
                }
            }

            return RedirectToAction(nameof(Index), nameof(HomeController));
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
        public async Task<IActionResult> SignUp(UserDTO userDTO)
        {
            //Check if phone number exists
            var existingUser = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == userDTO.PhoneNumber);
            if (existingUser != null)
            {
                ModelState.AddModelError("PhoneNumber", "هذا الرقم مسجل من قبل");
            }

            if (ModelState.IsValid)
            {

                if (userDTO.GovernmentId == -1)
                {
                    //Add Region And Government
                    GovernmentDTO existionGovernment = _governmentsBusiness.GetByName(userDTO.GovernmentName);
                    if (existionGovernment != null)
                        userDTO.GovernmentId = existionGovernment.Id;
                    else
                    {
                        //Add the government and get it;
                        GovernmentDTO newGovernment = new GovernmentDTO { Name = userDTO.GovernmentName };
                        var addResult = _governmentsBusiness.AddUpdate(newGovernment);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            userDTO.GovernmentId = _governmentsBusiness.GetByName(userDTO.GovernmentName).Id;
                        }
                    }
                }
                if (userDTO.RegionId == -1)
                {
                    //Add Region And Government
                    RegionDTO existionRegion = _regionsBusiness.GetByName(userDTO.RegionName);
                    if (existionRegion != null)
                        userDTO.RegionId = existionRegion.Id;
                    else
                    {
                        //Add the government and get it;
                        RegionDTO newRegion = new RegionDTO { Name = userDTO.RegionName };
                        var addResult = _regionsBusiness.AddUpdate(newRegion);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            userDTO.RegionId = _regionsBusiness.GetByName(userDTO.RegionName).Id;
                        }
                    }
                }


                ApplicationUser user = new ApplicationUser
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    PhoneNumber = userDTO.PhoneNumber,
                    UserName = userDTO.PhoneNumber,
                    Address = userDTO.Address,
                    RegionId = userDTO.RegionId,
                    GovernmentId = userDTO.GovernmentId,
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new string[] { Roles.Role_Customer });

                    //Add User Address
                    var userAddressDTO = new UserAddressDTO
                    {
                        UserId = user.Id,
                        Name = "المنزل",
                        GovernmentId = userDTO.GovernmentId,
                        RegionId = userDTO.RegionId,
                        Address = userDTO.Address,
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
                    SetTempMessage(ResponseTypes.Error, ResponseMessages.Error);
                    populateViewBags();
                    ViewData["ModelStateInvalid"] = true;
                    return View(userDTO);
                }

                SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessInsert);

                return RedirectToAction(nameof(PhoneNumberConfirmation), new { phoneNumber = userDTO.PhoneNumber });
            }
            populateViewBags();
            ViewData["ModelStateInvalid"] = true;
            return View(userDTO);
        }


        public IActionResult PhoneNumberConfirmation(string phoneNumber)
        {
            PhoneNumberConfirmationDTO phoneNumberConfirmationDTO = new PhoneNumberConfirmationDTO();
            phoneNumberConfirmationDTO.PhoneNumber = phoneNumber;

            //Send OTP
            Random random = new Random();
            string value = random.Next(1001, 9999).ToString();
            phoneNumberConfirmationDTO.OTPCode = value;
            _whatsAppAPIBusiness.SendOtpToWhatsApp(value, phoneNumber);

            return View(phoneNumberConfirmationDTO);
        }

        [HttpPost]
        public async Task<IActionResult> PhoneNumberConfirmation(PhoneNumberConfirmationDTO phoneNumberConfirmationDTO)
        {
            if (ModelState.IsValid)
            {
                var userOTP = phoneNumberConfirmationDTO.otp4 + phoneNumberConfirmationDTO.otp3 + phoneNumberConfirmationDTO.otp2 + phoneNumberConfirmationDTO.otp1;
                if (userOTP == phoneNumberConfirmationDTO.OTPCode)
                {
                    var user = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == phoneNumberConfirmationDTO.PhoneNumber);
                    if (user == null)
                    {
                        ModelState.AddModelError("OTPCode", "المستخدم غير مسجل");
                        return View(phoneNumberConfirmationDTO);
                    }
                    else
                    {
                        user.PhoneNumberConfirmed = true;
                        await _userManager.UpdateAsync(user);
                        SetTempMessage(ResponseTypes.Msg, "تم التسجيل بنجاح");
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("OTPCode", "رمز التأكيد خطأ");
                }
            }
            return View(phoneNumberConfirmationDTO);
        }


        [HttpGet]
        public IActionResult ResendOTPCode(string phoneNumber)
        {
            Random random = new Random();
            string value = random.Next(1001, 9999).ToString();
            _whatsAppAPIBusiness.SendOtpToWhatsApp(value, phoneNumber);

            return Json(new { newOTP = value });
        }


        [HttpGet]
        public IActionResult UserAccount()
        {
            var userId = GetUserId();
            if (userId == null)
                return NotFound();
            var user = _userManager.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
                return NotFound();

            UserDTO userDTO = new UserDTO
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                GovernmentId = user.GovernmentId,
                RegionId = user.RegionId

            };
            populateViewBags();
            ViewBag.Regions = _regionsBusiness.GetAll().Where(c => c.GovernmentId == user.GovernmentId).ToList();

            return View(userDTO);
        }
        [HttpPost]
        public async Task<IActionResult> UserAccount(UserDTO userDTO)
        {
            if (userDTO.PhoneNumber.Length == 11)
                userDTO.PhoneNumber = "2" + userDTO.PhoneNumber;

            if (ModelState.IsValid)
            {
                //check if user exists
                var user = await _userManager.FindByIdAsync(userDTO.Id);

                if (user == null)
                    return NotFound();

                var userWithSamePhoneNumber = _userManager.Users.FirstOrDefault(c => c.PhoneNumber == userDTO.PhoneNumber);

                if (userWithSamePhoneNumber != null && userWithSamePhoneNumber.Id.ToString() != userDTO.Id)
                {
                    ModelState.AddModelError("PhoneNumber", "الرقم مسجل من قبل");
                    populateViewBags();
                    ViewBag.Regions = _regionsBusiness.GetAll().Where(c => c.GovernmentId == user.GovernmentId).ToList();
                    return View(userDTO);
                }

                if (user.PhoneNumber != userDTO.PhoneNumber)
                {
                    user.PhoneNumber = userDTO.PhoneNumber;
                    user.PhoneNumberConfirmed = false;
                }

                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.UserName = userDTO.PhoneNumber;
                user.Address = userDTO.Address;
                user.GovernmentId = userDTO.GovernmentId;
                user.RegionId = userDTO.RegionId;


                await _userManager.UpdateAsync(user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _userManager.ResetPasswordAsync(user, token, userDTO.Password);


                SetTempMessage(ResponseTypes.Msg, ResponseMessages.SuccessUpdate);

                if (!user.PhoneNumberConfirmed)
                {
                    return RedirectToAction(nameof(PhoneNumberConfirmation), new { phoneNumber = userDTO.PhoneNumber });
                }
                return RedirectToAction(nameof(UserAccount));
            }

            return View(userDTO);
        }


        public IActionResult UserPaymentMethods()
        {
            var userId = GetUserId();
            if (userId == null)
                return NotFound();
            var user = _userManager.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
                return NotFound();

            var userPaymentMethods = _userPaymentMethodsBusiness.GetUserPaymentMethods(userId).ToList();

            UserDTO userDTO = new UserDTO
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                GovernmentId = user.GovernmentId,
                RegionId = user.RegionId,
                UserPaymentMethods = userPaymentMethods

            };

            return View(userDTO);
        }

        public IActionResult UserAddresses()
        {
            var userId = GetUserId();
            if (userId == null)
                return NotFound();
            var user = _userManager.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
                return NotFound();

            var userAddresses = _userAddressesBusiness.GetUserAddresses(userId).ToList();

            UserDTO userDTO = new UserDTO
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                GovernmentId = user.GovernmentId,
                RegionId = user.RegionId,
                UserAddresses = userAddresses

            };
            populateViewBags();
            ViewBag.AddressTypes = setSelectList(_addressTypesBusiness.GetAll().ToList(), "Id", "Name");
            return View(userDTO);
        }


        [HttpPost]
        public IActionResult AddNewPaymentMethod(UserPaymentMethodDTO userPaymentMethodDTO)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                if (userPaymentMethodDTO.isDefault)
                {
                    //remove default from others
                    List<UserPaymentMethodDTO> userPaymentMethods = _userPaymentMethodsBusiness.GetUserPaymentMethods(GetUserId()).Where(c => c.Id != userPaymentMethodDTO.Id).ToList();
                    if (userPaymentMethods != null)
                    {
                        userPaymentMethods.ForEach(paymentMethod => paymentMethod.isDefault = false);
                        _userPaymentMethodsBusiness.AddUpdateRange(userPaymentMethods);
                    }
                }

                result = _userPaymentMethodsBusiness.AddUpdate(userPaymentMethodDTO);



                setUpsertTempMessages(result);

                return RedirectToAction(nameof(UserPaymentMethods));
            }
            return View(userPaymentMethodDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewAddress(UserAddressDTO userAddressDTO)
        {
            if (ModelState.IsValid)
            {
                var result = 0;

                if (userAddressDTO.GovernmentId == -1)
                {
                    //Add Region And Government
                    GovernmentDTO existionGovernment = _governmentsBusiness.GetByName(userAddressDTO.GovernmentName);
                    if (existionGovernment != null)
                        userAddressDTO.GovernmentId = existionGovernment.Id;
                    else
                    {
                        //Add the government and get it;
                        GovernmentDTO newGovernment = new GovernmentDTO { Name = userAddressDTO.GovernmentName };
                        var addResult = _governmentsBusiness.AddUpdate(newGovernment);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            userAddressDTO.GovernmentId = _governmentsBusiness.GetByName(userAddressDTO.GovernmentName).Id;
                        }
                    }
                }
                if (userAddressDTO.RegionId == -1)
                {
                    //Add Region And Government
                    RegionDTO existionRegion = _regionsBusiness.GetByName(userAddressDTO.RegionName);
                    if (existionRegion != null)
                        userAddressDTO.RegionId = existionRegion.Id;
                    else
                    {
                        //Add the government and get it;
                        RegionDTO newRegion = new RegionDTO { Name = userAddressDTO.RegionName };
                        var addResult = _regionsBusiness.AddUpdate(newRegion);
                        if (addResult == (int)SignatureResponseMessage.SuccessInsert)
                        {
                            userAddressDTO.RegionId = _regionsBusiness.GetByName(userAddressDTO.RegionName).Id;
                        }
                    }
                }

                var user = _userManager.Users.FirstOrDefault(c => c.Id == userAddressDTO.UserId);
                if (user != null)
                {
                    if (userAddressDTO.oldAddress == user.Address)
                    {
                        //Update user data in address tables
                        user.Address = userAddressDTO?.Address;
                        user.RegionId = userAddressDTO?.RegionId;
                        user.GovernmentId = userAddressDTO?.GovernmentId;

                        await _userManager.UpdateAsync(user);
                    }

                }



                result = _userAddressesBusiness.AddUpdate(userAddressDTO);

                setUpsertTempMessages(result);

                return RedirectToAction(nameof(UserAddresses));
            }
            return View(userAddressDTO);
        }


        public ActionResult GetPaymentDetails(int id)
        {
            // Get the payment method details based on the ID
            var paymentMethod = _userPaymentMethodsBusiness.GetById(id);
            if (paymentMethod == null)
            {
                paymentMethod = new UserPaymentMethodDTO
                {
                    userId = GetUserId(),
                };
            }
            // Return the partial view with the payment method details
            return PartialView("_AddNewPaymentMethod", paymentMethod); // _PaymentDetailsPartial is the name of your partial view
        }
        public ActionResult GetAddressDetails(int id)
        {
            // Get the payment method details based on the ID
            var address = _userAddressesBusiness.GetById(id);
            if (address == null)
            {
                address = new UserAddressDTO
                {
                    UserId = GetUserId(),
                };

            }
            else
            {
                address.oldAddress = address.Address;
                ViewBag.Regions = setSelectList(_regionsBusiness.GetAll().Where(c => c.GovernmentId == address.GovernmentId).ToList(), "Id", "Name");

            }

            populateViewBags();
            ViewBag.AddressTypes = setSelectList(_addressTypesBusiness.GetAll().ToList(), "Id", "Name");

            // Return the partial view with the payment method details
            return PartialView("_AddNewAddress", address); // _PaymentDetailsPartial is the name of your partial view
        }


        [HttpPost]
        public ActionResult DeletePaymentMethod(int id)
        {
            // Your service logic to delete the payment method by id
            bool inRelation = hasRelation(id.ToString(), "UserPaymentMethodId");
            var result = _userPaymentMethodsBusiness.Delete(id, inRelation);

            if (result)
            {
                // Return success response
                return Json(new { success = true });
            }
            else
            {
                // Return failure response
                return Json(new { success = false, message = "حدث خطأ" });
            }
        }

        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            // Your service logic to delete the payment method by id
            bool inRelation = hasRelation(id.ToString(), "UserAddressId");
            var result = _userAddressesBusiness.Delete(id, inRelation);

            if (result)
            {
                // Return success response
                return Json(new { success = true });
            }
            else
            {
                // Return failure response
                return Json(new { success = false, message = "حدث خطأ" });
            }
        }

    }
}
