using Foody.DTOs;
using Foody.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto model);
        Task<Response<RegisterResponseDto>> Register(RegisterDto model, string scheme, IUrlHelper url);
        Task<bool> ConfirmEmail(string userid, string token);

        Task<bool> SendResetPasswordLink(string email, IUrlHelper url, string scheme);

        Task<Response<string>> ResetPassword(ResetPasswordDto resetpassword);
    }
}
