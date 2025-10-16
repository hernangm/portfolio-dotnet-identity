using Portfolio.Dotnet.Identity.Users.Models;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;
using System.Security.Claims;

namespace Portfolio.Dotnet.Identity.Users.Contracts
{
    public interface IUserService
    {
        Task<UsersOperationResponse> AddOrUpdateClaims(AddOrUpdateClaimsRequest request);
        Task<UsersOperationResponse> ChangePassword(ChangePasswordRequest request);
        Task<UserDTO?> FindByExternalProvider(string loginProvider, string providerKey);
        Task<UserDTO?> GetUserByUserName(string userName);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO?> LinkUser(string provider, string userId, List<Claim> claims);
        Task<UsersOperationResponse> RemoveClaims(RemoveClaimsRequest request);
        Task<UsersOperationResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request);
        Task<UsersOperationResponse> SetPasswordWithoutToken(SetPasswordRequest request);
        Task<UsersOperationResponse> SetPasswordWithToken(SetPasswordWithTokenRequest request);
        bool UserNameExists(string userName);
        Task<bool> ValidateCredentials(string? userName, string? password);
        Task<UsersOperationResponse> ValidatePassword(string? password);
    }
}
