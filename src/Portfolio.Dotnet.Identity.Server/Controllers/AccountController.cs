using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;

namespace Portfolio.Dotnet.Identity.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(LocalApi.PolicyName)]
    public partial class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService UserService = userService;

        [HttpPost("validatePassword")]
        public async Task<UsersOperationResponse> ValidatePassword([FromBody] ValidatePasswordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            var validatePasswordResponse = await UserService.ValidatePassword(request.Password ?? string.Empty);
            return validatePasswordResponse;
        }

        [HttpPatch("changePassword")]
        public async Task<UsersOperationResponse> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(request.CurrentPassword))
            {
                errors.Add("You must enter the current password.");
            }
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                errors.Add("You must enter the new password.");
            }
            if (errors.Count > 0)
            {
                return new UsersOperationResponse(false, errors);
            }
            var changePasswordResponse = await UserService.ChangePassword(request);
            return changePasswordResponse;
        }


        [HttpPost("sendResetPasswordEmail")]
        [AllowAnonymous]
        public async Task<UsersOperationResponse> SendResetPasswordEmail([FromBody] SendResetPasswordEmailRequest request)
        {
            var result = await UserService.SendResetPasswordEmail(request);
            return result;
        }


        [HttpPatch("setPassword")]
        [AllowAnonymous]
        public async Task<UsersOperationResponse> SetPassword([FromBody] SetPasswordWithTokenRequest request)
        {
            var setPasswordResult = await UserService.SetPasswordWithToken(request);
            return setPasswordResult;
        }


        [HttpPatch("setPasswordWithoutToken")]
        //[AllowAnonymous]
        public async Task<UsersOperationResponse> SetPasswordWithoutToken([FromBody] SetPasswordRequest request)
        {
            var setPasswordResult = await UserService.SetPasswordWithoutToken(request);
            return setPasswordResult;
        }
    }
}
