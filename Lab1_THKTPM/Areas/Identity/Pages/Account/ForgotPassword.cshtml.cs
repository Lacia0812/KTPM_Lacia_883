// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Lab1_THKTPM.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Message { get; set; } // Thông báo gửi về view
        public bool IsSuccess { get; set; } // Xác định trạng thái thành công hay lỗi

        public class InputModel
        {
            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                Message = "Email không tồn tại trong hệ thống.";
                IsSuccess = false;
                return Page();
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                Message = "Email chưa được xác nhận. Vui lòng xác nhận email trước khi đặt lại mật khẩu.";
                IsSuccess = false;
                return Page();
            }

            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { userId = user.Id, code = encodedCode },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Đặt lại mật khẩu",
                    $"Để đặt lại mật khẩu, vui lòng nhấn vào link sau: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Đặt lại mật khẩu</a>."
                );

                _logger.LogInformation("Email đặt lại mật khẩu đã được gửi đến {Email}.", Input.Email);

                Message = "Một email đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra hộp thư.";
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Lỗi khi gửi email: {Message}", ex.Message);
                Message = "Có lỗi xảy ra khi gửi email. Vui lòng thử lại sau.";
                IsSuccess = false;
            }

            return Page();
        }
    }
}
