// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contracts.Seeds;
using Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Slipperty.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShoppingCartsBusiness _shoppingCartsBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager/*, ITenantsBusiness tenantsBusiness*/,
            IWebHostEnvironment env, IConfiguration configuration/*, ITenantService tenantService*/, IShoppingCartsBusiness shoppingCartsBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _shoppingCartsBusiness = shoppingCartsBusiness;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            //[EmailAddress]
            [Display(Name = "EmailorUsername")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Rememberme")]
            public bool RememberMe { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(Roles.Role_Admin))
                {
                    // Redirect to a specific page if the user is already authenticated
                    Response.Redirect("/Admin/Home/Index");
                    return;
                }
            }


            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            //if(User.IsInRole(Roles.Role_Admin))
            //{
            //    returnUrl = "~/Admin/Home/Index";
            //    ReturnUrl = returnUrl;
            //}

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");



            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            var username = Input.Email;

            //var isValidEmail = new EmailAddressAttribute().IsValid(Input.Email);

            if (Input.Email.Length == 11)
                Input.Email = "2" + Input.Email;


            var user = await _userManager.Users.Where(c => c.PhoneNumber == Input.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                username = user.UserName;
            }


            //check if user exists 

            Input.RememberMe = true;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    
                    if (User.IsInRole(Roles.Role_Admin))
                        returnUrl = "~/Admin/Home/Index";
                    else
                    {
                        //is normal user so merge carts
                        _shoppingCartsBusiness.MergeSessionCartToUser(GetGuestSessionId(), user.Id);

                        if (user.AutoRegistered)
                        {
                            returnUrl = $"~/User/UserManagement/SetPassword?userId={user.Id}";
                        }
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "رقم الهاتف او كلمة المرور غير صحيحة");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
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
    }
}
