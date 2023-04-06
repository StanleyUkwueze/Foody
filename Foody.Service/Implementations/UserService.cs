using AutoMapper;
using Foody.DTOs;
using Foody.Model.Models;
using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IMapper _mapper;
        private readonly IphotoService _photoservice;

        public UserService(UserManager<Customer> userManager, IMapper mapper, IphotoService photoservice)
        {
            _userManager = userManager;
            _mapper = mapper;
            _photoservice = photoservice;
        }
        public async Task<bool> DeleteUser(string Id)
        {
       var userToDelete = await _userManager.FindByIdAsync(Id);
            if(userToDelete != null)
            {
             var result =   await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

      
        public PagedResponse<AdminUserDTO> GetAllUser(PagingParameter param)
        {
            var pagedResult = _userManager.Users.Paginate(param.PageNumber, param.PageSize);
            if(pagedResult == null)
            {
                throw new Exception("No Record is found");
            }

            var mappedUsers = _mapper.Map<PagedResponse<Customer>, PagedResponse<AdminUserDTO>>(pagedResult);

            return mappedUsers;
        }

        public async Task<Response<UserDTO>> GetUserByEmail(string email)
        {
            Response<UserDTO> response = new Response<UserDTO>();
            var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
                {
                response.IsSuccessful = false;
                response.Message = "User not found";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
                }
           var mappedUser = _mapper.Map<Customer, UserDTO>(user);

            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "User successfully retrieved";
            response.Data = mappedUser;

            return response;
        }

        public async Task<Response<UserDTO>> GetUserById(string Id)
        {
            Response<UserDTO> response = new Response<UserDTO>();
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "User not found";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            var mappedUser = _mapper.Map<Customer, UserDTO>(user);

            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "User successfully retrieved";
            response.Data = mappedUser;

            return response;
        }

        public async Task<Response<UserDTO>> UpdateUser(UpdateUserDto user)
        {
            Response<UserDTO> response = new Response<UserDTO>();
            var userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if(userToUpdate == null)
            {
                response.Message = "User not found";
                response.IsSuccessful =false;
                response.StatusCode = (int)HttpStatusCode.NotFound;

                return response;
            }

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Gender =user.Gender;
            userToUpdate.Email = user.Email;

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (result.Succeeded)
            {
                response.Message = "User successfully updated";
                response.IsSuccessful = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                
                return response;
            }
            response.Message = "User update failed kindly try again";
            response.IsSuccessful = false;
            response.StatusCode = 400;

            return response;
        }

        public async Task<Response<string>> UploadPhoto(IFormFile file, string Id)
        {
            Response<string> response = new Response<string>();
            UploadAvatarResponse uploadAvatarResponse = new UploadAvatarResponse();
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null) 
                return new Response<string> { Message = "User does not exist", StatusCode = 404, IsSuccessful = false };
            if (file.Length > 0 && file != null)
            {
               var photoResult = await _photoservice.AddPhotoAsync(file);

                user.AvatarUrl = photoResult.Url.ToString();
                user.publicId = photoResult.PublicId;

                await _userManager.UpdateAsync(user);

                response.Message = "Photo uploaded successfully";
                response.StatusCode = (int)HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Data = $"{user.publicId} {user.AvatarUrl}";
                return response;
            }
            response.Message = "Kindly select a photo";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.IsSuccessful = false;
            return response;
        }
    }
}
