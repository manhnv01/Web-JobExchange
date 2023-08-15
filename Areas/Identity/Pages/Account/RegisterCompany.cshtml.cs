// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using JobExchange.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using JobExchange.Repository.RepositoryInterfaces;
using JobExchange.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Areas.Identity.Pages.Account
{
    public class RegisterCompanyModel : PageModel
    {
        private readonly SignInManager<JobExchangeUser> _signInManager;
        private readonly UserManager<JobExchangeUser> _userManager;
        private readonly IUserStore<JobExchangeUser> _userStore;
        private readonly IUserEmailStore<JobExchangeUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly JobExchangeContext _context;

        public RegisterCompanyModel(
            JobExchangeContext context,
            UserManager<JobExchangeUser> userManager,
            IUserStore<JobExchangeUser> userStore,
            SignInManager<JobExchangeUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Model register
        public class InputModel
        {
            [Required (ErrorMessage = "Vui lòng nhập Email của bạn.")]
            [EmailAddress (ErrorMessage = "Không phải địa chỉ Email!")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập Mật khẩu.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Mật khẩu phải có 8 ký tự trở lên bao gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt!")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp!")]
            public string ConfirmPassword { get; set; }

            [Required (ErrorMessage = "Hãy cho chúng tôi biết tên của bạn.")]
            public string AccountName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập tên công ty.")]
            public string CompanyName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập địa chỉ công ty.")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số điện thoại công ty.")]
            [RegularExpression(@"^((01(\d){8})|(03(\d){8})|(07(\d){8})|(08(\d){8})|(09(\d){8}))$", ErrorMessage = "Không phải số điện thoại Việt Nam!")]
            public string Phone { get; set; }

            [Range( 1, 100000000, ErrorMessage = "Vui lòng nhập lĩnh vực hoạt động.")]
            public int IndustryId { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.AccountName = Input.AccountName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    await _userManager.AddToRoleAsync(user, "ROLE_COMPANY");

                    Company company = new Company();
                    company.CompanyId = userId;
                    company.AccountId = userId;
                    company.CompanyName = Input.CompanyName;
                    company.Address = Input.Address;
                    company.Phone = Input.Phone;
                    company.IndustryId = Input.IndustryId;
                    company.Avatar = "avatar-default.jpg";
                    company.CoverImage = "profile_default_cover.jpg";
                    ViewData["IndustryId"] = new SelectList(_context.Industries, "IndustryId", "IndustryName");
                    _context.Add(company);
                    _context.SaveChanges();

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private JobExchangeUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<JobExchangeUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(JobExchangeUser)}'. " +
                    $"Ensure that '{nameof(JobExchangeUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<JobExchangeUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<JobExchangeUser>)_userStore;
        }
    }
}
