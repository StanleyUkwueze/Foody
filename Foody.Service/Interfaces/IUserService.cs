using Foody.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserDTO>> UpdateUser(UpdateUserDto user);

        Task<bool> DeleteUser(string Id);

        Task<Response<UserDTO>> GetUserById(string Id);

        Task<Response<UserDTO>> GetUserByEmail(string Id);

        Task<Response<string>> UploadPhoto(IFormFile file, string Id);
        PagedResponse<AdminUserDTO> GetAllUser(PagingParameter param);
    }
}
