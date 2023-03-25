using AutoMapper;
using Foody.DataAcess;
using Foody.DTOs;
using Foody.Model.Models;
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
using System.Threading.Tasks;

namespace Foody.Service
{
    public class AuthService : IAuthService
    {
        private UserManager<Customer> _userManager;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        //private readonly IMailService _mailService;
        private readonly SignInManager<Customer> _signInManager;


        public AuthService(IServiceProvider serviceProvider, SignInManager<Customer> signInManager)
        {
            _jwtService = serviceProvider.GetRequiredService<IJWTService>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Customer>>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _context= serviceProvider.GetRequiredService<AppDbContext>();
            // _mailService = serviceProvider.GetRequiredService<IMailService>();
            _signInManager = signInManager;

        }
        public Task<bool> ConfirmEmail(string userid, string token)
        {
            throw new NotImplementedException();
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
                    var roles = await _userManager.GetRolesAsync(user);

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

            var customer = new Customer
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName  = model.LastName,
                Email = model.Email,
                Gender = model.Gender,

            };

      
                //_mapper.Map<RegisterDto,Customer>(model);

            //create a new shopping cart
           
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
            
            var urlToBeSentTocustomer = url.Action("ConfirmEmail", "Account", new { token = validToken, email = customer.Email }, scheme);

            //send the mail here
            //if email failed to send, then delete cutomer from db
        
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
    }
}
