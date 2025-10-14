using Portfolio.Dotnet.Identity.Users.Models;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;
using System.Security.Claims;

namespace Portfolio.Dotnet.Identity.Users.Contracts
{
    public interface IUserService
    {
        Task<UsersOperationResponse> AddOrUpdateClaims(AddOrUpdateClaimsRequest request);
        //Task<UsersOperationResponse> ChangeEmail(ChangeEmailRequest request);
        Task<UsersOperationResponse> ChangePassword(ChangePasswordRequest request);
        //Task<UsersOperationResponse> ChangePhoneNumber(ChangePhoneNumberRequest request);
        //Task<UsersOperationResponse> DeleteUser(DeleteUserRequest request);
        Task<UserDTO?> FindByExternalProvider(string loginProvider, string providerKey);
        Task<UserDTO?> GetUserByUserName(string userName);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO?> LinkUser(string provider, string userId, List<Claim> claims);
        Task<UsersOperationResponse> RemoveClaims(RemoveClaimsRequest request);
        Task<UsersOperationResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request);
        Task<UsersOperationResponse> SetPasswordWithoutToken(SetPasswordRequest request);
        Task<UsersOperationResponse> SetPasswordWithToken(SetPasswordWithTokenRequest request);
        //Task<UsersOperationResponse> UpdateUser(UpdateUserRequest request);
        bool UserNameExists(string userName);
        Task<bool> ValidateCredentials(string? userName, string? password);
        Task<UsersOperationResponse> ValidatePassword(string? password);
        //Task<UsersOperationResponse> UpdateClaim(UpdateClaimRequest request);
    }
}
