using AutoMapper;
using Foody.Commons;
using Foody.DataAcess;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using Foody.Service.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private UserManager<Customer> _userManager;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IMailService _mailService;
        private readonly SignInManager<Customer> _signInManager;


        public AuthService(IServiceProvider serviceProvider, SignInManager<Customer> signInManager, IMailService mailService)
        {
            _jwtService = serviceProvider.GetRequiredService<IJWTService>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _context = serviceProvider.GetRequiredService<AppDbContext>();
            // _mailService = serviceProvider.GetRequiredService<IMailService>();
            _signInManager = signInManager;
            _mailService = mailService;
        }
        public async Task<bool> ConfirmEmail(string useremail, string token)
        {
            Customer user = await _userManager.FindByEmailAsync(useremail);
            if (user == null)
            {
                throw new AccessViolationException("Access Denied");
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto model)
        {
            LoginResponseDto response = new LoginResponseDto();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.Message = "No record found";
                response.Status = false;
                return response;
            }

            var result = await _userManager.IsEmailConfirmedAsync(user);

            if (result)
            {
                var res = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (res.Succeeded)
                {
                    //var roles = await _userManager.GetRolesAsync(user);

                    var token = await _jwtService.GenerateJwtToken(user);


                    return new LoginResponseDto { Id = user.Id, Status = true, Token = token, Message = "Successfully loged In!" };
                }


                return new LoginResponseDto
                {
                    Status = false,
                    Message = "Sign in Failed! Check your credentials and try again."
                };
            }

            return new LoginResponseDto
            { Status = false, Message = "Email not Confirmed" };

        }

        public async Task<Response<RegisterResponseDto>> Register(RegisterDto model, string scheme, IUrlHelper url)
        {
            Response<RegisterResponseDto> response = new Response<RegisterResponseDto>();

            var checkuser = await _userManager.FindByEmailAsync(model.Email);
            if (checkuser != null) throw new BadHttpRequestException("user with the email already exist");

            var customer = _mapper.Map<RegisterDto, Customer>(model);

            var result = await _userManager.CreateAsync(customer, model.Password);

            if (!result.Succeeded)
            {
                throw new BadHttpRequestException("Customer not successfully created");
            }

            //generate email confirmation token
            //encode the token
            //validate the token


            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(customer);
            var encodedToken = Encoding.UTF8.GetBytes(emailConfirmationToken);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            var urlToBeSentTocustomer = url.Action("ConfirmEmail", "Auth", new { token = validToken, email = customer.Email }, scheme);

            //send the mail here
            //if email failed to send, then delete cutomer from db
            var name = $"{model.FirstName} {model.LastName}";
            var res = await SendMailAsync(model.Email, "Email Confirmation", $"Kindly click the link to confirm your email <a href=`{HtmlEncoder.Default.Encode(urlToBeSentTocustomer)}`>Click here</a>");

            if (!res)
            {
               await _userManager.DeleteAsync(customer);
                response.Message = "Mail not confirmed";
                response.IsSuccessful = false;

                return response;
            }

            response.StatusCode = (int)HttpStatusCode.Created;
            response.Message = "Registration successful";
            response.IsSuccessful = true;
            response.Data = new RegisterResponseDto
            {
                Id = customer.Id,
                UserName = customer.UserName
            };
            return response;
        }

        public Task<Response<string>> ResetPassword(ResetPasswordDto resetpassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendResetPasswordLink(string email, IUrlHelper url, string scheme)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SendMailAsync(string recipientmail, string subject, string body )
        {
            var mailrequest = new MailRequest
            {
               // Name = name,
                Subject = subject,
                Body = body,
              //  Link = link,
                RecipientMail = recipientmail

            };
            await _mailService.SendEmailAsync(recipientmail, subject, body);
            return true;
        }
    }
}
