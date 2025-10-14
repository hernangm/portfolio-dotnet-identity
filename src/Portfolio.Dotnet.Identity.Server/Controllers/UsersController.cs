using Microsoft.AspNetCore.Mvc;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Models;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;

namespace Portfolio.Dotnet.Identity.Server.Controllers
{
    [Route("users")]
    [ApiController]
    public partial class UsersController(IUserService userService, IPasswordGeneratorService passwordGeneratorService) : ControllerBase
    {
        private readonly IUserService UserService = userService;
        private readonly IPasswordGeneratorService PasswordGeneratorService = passwordGeneratorService;

        [HttpPost("{userName}/claims")]
        public async Task<UsersOperationResponse> AddOrUpdateClaims(string userName, [FromBody] AddOrUpdateClaimsRequest request)
        {
            request.UserName = userName;
            var response = await UserService.AddOrUpdateClaims(request);
            return response;
        }

        [HttpDelete("{userName}/claims")]
        public async Task<UsersOperationResponse> RemoveClaims(string userName, [FromBody] RemoveClaimsRequest request)
        {
            request.UserName = userName;
            var response = await UserService.RemoveClaims(request);
            return response;
        }


        //[HttpGet("id/{id:int}")]
        //public async Task<UserDTO?> GetUserById(int id)
        //{
        //    var response = await UserService.GetUserById(id);
        //    return response;
        //}


        [HttpGet("userName/{userName}")]
        public async Task<UserDTO?> GetUserByUserName(string userName)
        {
            var response = await UserService.GetUserByUserName(userName);
            return response;
        }


        [HttpGet("")]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await UserService.GetUsers();
            return users;
        }


        [HttpGet("userName/{userName}/exists")]
        public bool UserNameExists(string userName)
        {
            var exists = UserService.UserNameExists(userName);
            return exists;
        }


        [HttpGet("password")]
        public GeneratePasswordResponse GeneratePassword()
        {
            var password = PasswordGeneratorService.GeneratePassword();
            return new GeneratePasswordResponse { Password = password };
        }
    }
}