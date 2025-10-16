using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Dotnet.Identity.Data.Users;
using Portfolio.Dotnet.Identity.Users.Contracts;
using Portfolio.Dotnet.Identity.Users.Models;
using Portfolio.Dotnet.Identity.Users.Models.Requests;
using Portfolio.Dotnet.Identity.Users.Models.Responses;
using System.Data;
using System.Security.Claims;
using System.Web;

namespace Portfolio.Dotnet.Identity.Users
{
    public class UserService(
              UserManager<ThisUser> userManager
            , IPasswordGeneratorService passwordGenerator
            , ILoggerFactory loggerFactory
        ) : IUserService
    {
        private readonly ILogger<UserService> Logger = loggerFactory.CreateLogger<UserService>();
        private readonly IPasswordGeneratorService PasswordGenerator = passwordGenerator;
        private readonly UserManager<ThisUser> UserManager = userManager;

        public async Task<UsersOperationResponse> AddOrUpdateClaims(AddOrUpdateClaimsRequest request)
        {
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User not found.");
            }
            var claims = request.Claims ?? [];
            var results = new List<UsersOperationResponse>();
            foreach (var claim in claims)
            {
                var r = await AddOrUpdateUserClaim(user, claim.ClaimType, claim.ClaimValue);
                results.Add(r);
            }
            return new UsersOperationResponse(results.All(m => true), string.Join('|', results.SelectMany(m => m.Errors)));
        }

        public async Task<UsersOperationResponse> RemoveClaims(RemoveClaimsRequest request)
        {
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User not found.");
            }
            var claims = request.Claims ?? [];
            var results = new List<UsersOperationResponse>();
            foreach (var claim in claims)
            {
                var r = await RemoveUserClaim(user, claim.ClaimType, claim.ClaimValue);
                results.Add(r);
            }
            return new UsersOperationResponse(results.All(m => true), string.Join('|', results.SelectMany(m => m.Errors)));
        }

        //public async Task<UsersOperationResponse> ChangeEmail(ChangeEmailRequest request)
        //{
        //    var user = await GetUser(request.UserName);
        //    if (user == null)
        //    {
        //        return new UsersOperationResponse("User doesn't exist.");
        //    }
        //    var setEmailResult = await UserManager.SetEmailAsync(user, request.Email);
        //    if (!setEmailResult.Succeeded)
        //    {
        //        return new UsersOperationResponse(setEmailResult.Succeeded, setEmailResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        //    }
        //    var updateResult = await UserManager.UpdateAsync(user);
        //    return new UsersOperationResponse(updateResult.Succeeded, updateResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        //}

        //public async Task<UserDTO?> AutoProvisionUser(string provider, string userId, List<Claim> claims)
        //{
        //    // create a list of claims that we want to transfer into our store
        //    var filtered = new List<Claim>();
        //    foreach (var claim in claims)
        //    {
        //        // if the external system sends a display name - translate that to the standard OIDC name claim
        //        if (claim.Type == ClaimTypes.Name)
        //        {
        //            filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
        //        }
        //        // if the JWT handler has an outbound mapping to an OIDC claim use that
        //        else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type))
        //        {
        //            filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
        //        }
        //        // copy the claim as-is
        //        else
        //        {
        //            filtered.Add(claim);
        //        }
        //    }
        //    // if no display name was provided, try to construct by first and/or last name
        //    if (!filtered.Any(x => x.Type == JwtClaimTypes.Name))
        //    {
        //        var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
        //        var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
        //        if (first != null && last != null)
        //        {
        //            filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
        //        }
        //        else if (first != null)
        //        {
        //            filtered.Add(new Claim(JwtClaimTypes.Name, first));
        //        }
        //        else if (last != null)
        //        {
        //            filtered.Add(new Claim(JwtClaimTypes.Name, last));
        //        }
        //    }
        //    // create a new unique subject id
        //    var sub = CryptoRandom.CreateUniqueId(format: CryptoRandom.OutputFormat.Hex);
        //    // check if a display name is available, otherwise fallback to subject id
        //    var name = filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub;
        //    // create new user
        //    var user = new ThisUser
        //    {
        //        UserName = name,
        //        //SubjectId = sub,
        //        UserClaims = filtered.Select(c => new ThisUserClaim()
        //        {
        //            ClaimType = c.Type,
        //            ClaimValue = c.Value
        //        }).ToList(),
        //        UserLogins = new List<ThisUserLogin> {
        //            new ThisUserLogin {
        //                LoginProvider = provider
        //            }
        //        }
        //    };
        //    var r = await UserManager.CreateAsync(user) ?? throw new Exception("something happened when attempting to create a user");
        //    var result = MapUserDTO(user, null);
        //    return result;
        //}
        public async Task<UsersOperationResponse> ChangePassword(ChangePasswordRequest request)
        {
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User doesn't exist.");
            }
            var result = await UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            // TODO: check if is possible to revoke the current token
            //var sendEmail = request.SendEmail ?? true;
            //if (result.Succeeded && sendEmail && !string.IsNullOrEmpty(user.Email))
            //{
            //    await EmailSender.SendPasswordChangedEmail(new EmailRecipient(user.Email), new Email.Emails.PasswordChanged.PasswordChangedEmailModel { });
            //}
            return new UsersOperationResponse(result.Succeeded, result.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        }

        //public async Task<UsersOperationResponse> ChangePhoneNumber(ChangePhoneNumberRequest request)
        //{
        //    var user = await GetUser(request.UserName);
        //    if (user == null)
        //    {
        //        return new UsersOperationResponse("User doesn't exist.");
        //    }
        //    var setPhoneNumberResult = await UserManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        //    if (!setPhoneNumberResult.Succeeded)
        //    {
        //        return new UsersOperationResponse(setPhoneNumberResult.Succeeded, setPhoneNumberResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        //    }
        //    var updateResult = await UserManager.UpdateAsync(user);
        //    return new UsersOperationResponse(updateResult.Succeeded, updateResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        //}
        //public async Task<UserCreationOperationResponse> CreateUser(CreateUserRequest request)
        //{
        //    ArgumentNullException.ThrowIfNull(request);
        //    var user = await GetUser(request.UserName);
        //    if (user == null)
        //    {
        //        user = new ThisUser
        //        {
        //            UserName = request.UserName,
        //            Email = request.Email,
        //            PhoneNumber = request.PhoneNumber,
        //            UserClaims = request.Claims?.Select(c => new ThisUserClaim()
        //            {
        //                ClaimType = c.Type,
        //                ClaimValue = c.Value
        //            }).ToList() ?? []
        //        };
        //        var passwordNeedsToBeReset = string.IsNullOrEmpty(request.Password);
        //        if (passwordNeedsToBeReset)
        //        {
        //            request.Password = PasswordGenerator.GeneratePassword();
        //        }
        //        var createUserResult = await UserManager.CreateAsync(user, request.Password ?? string.Empty) ?? throw new Exception("something happened when attempting to create a user");
        //        if (!createUserResult.Succeeded)
        //        {
        //            return new UserCreationOperationResponse(createUserResult.Succeeded, createUserResult.Errors?.Select(e => e.Description) ?? []);
        //        }
        //        var resetPasswordToken = string.Empty;
        //        if (request.CreateResetAccountToken)
        //        {
        //            resetPasswordToken = await CreateResetPasswordToken(user, request.ResetPasswordUrl ?? string.Empty);
        //        }
        //        var sendEmail = request.SendEmail ?? true;
        //        if (sendEmail)
        //        {
        //            SendEmailResponse sendEmailResponse;
        //            if (passwordNeedsToBeReset)
        //            {
        //                if (string.IsNullOrEmpty(resetPasswordToken))
        //                {
        //                    resetPasswordToken = await CreateResetPasswordToken(user, request.ResetPasswordUrl ?? string.Empty);
        //                }
        //                sendEmailResponse = await SendResetPasswordEmail(user.Email, resetPasswordToken);
        //            }
        //            else
        //            {
        //                sendEmailResponse = await EmailSender.SendAccountCreatedEmail(new EmailRecipient(user.Email), new Email.Emails.AccountCreated.AccountCreatedEmailModel { });
        //            }
        //            if (!sendEmailResponse.Successful)
        //            {
        //                Logger.LogWarning("Could not send email to {recipient}", user.Email);
        //            }
        //        }
        //        return new UserCreationOperationResponse(createUserResult.Succeeded, createUserResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"])
        //        {
        //            UserId = UserManager.Users.Single(m => m.UserName == request.UserName).Id
        //        };
        //    }
        //    else
        //    {
        //        foreach (var claim in request.Claims)
        //        {
        //            var userClaim = user.GetClaim(claim.Type);
        //            if (userClaim == null)
        //            {
        //                var claimToAdd = new ThisUserClaim()
        //                {
        //                    ClaimType = claim.Type,
        //                    ClaimValue = claim.Value
        //                };
        //                user.UserClaims.Add(claimToAdd);
        //            }
        //            else
        //            {
        //                userClaim.ClaimValue = claim.Value;
        //            }
        //        }
        //        var updateUserResult = await UserManager.UpdateAsync(user);
        //        return new UserCreationOperationResponse(updateUserResult.Succeeded, updateUserResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"])
        //        {
        //            UserId = user.Id
        //        };
        //    }
        //}


        //public async Task<UsersOperationResponse> DeleteUser(DeleteUserRequest request)
        //{
        //    var user = await GetUser(request.UserName);
        //    if (user == null)
        //    {
        //        return new UsersOperationResponse(false, "User doesn't exist.");
        //    }
        //    var resultDelete = await UserManager.DeleteAsync(user);
        //    return new UsersOperationResponse(resultDelete.Succeeded, resultDelete.Errors?.Select(e => e.Description).ToList() ?? ["Unexpected error"]);
        //}

        public async Task<UserDTO?> FindByExternalProvider(string? loginProvider, string? providerKey)
        {
            if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(providerKey))
            {
                return null;
            }
            var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);
            if (user is null)
            {
                return null;
            }
            var result = MapUserDTO(user);
            return result;
        }

        public async Task<UserDTO?> GetUserByUserName(string userName)
        {
            var user = await GetUser(userName);
            if (user != null)
            {
                return MapUserDTO(user);
            }
            return null;
        }


        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var trueValue = true.ToString().ToLower();
            var users = await UserManager.Users
                .Include(u => u.UserClaims)
                 .ToListAsync();
            var result = users.Select(MapUserDTO);
            return result;
        }

        public async Task<UserDTO?> LinkUser(string provider, string providerKey, List<Claim> claims)
        {
            var user = await UserManager.FindByLoginAsync(provider, providerKey);
            if (user != null)
            {
                var result = MapUserDTO(user);
                return result;
            }
            var claimTypes = new List<string> {
               ClaimTypes.Name,
               ClaimTypes.Email
            };
            foreach (var ct in claimTypes)
            {
                var email = claims.FirstOrDefault(m => m.Type == ct)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    user = await UserManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        break;
                    }
                }
            }
            if (user != null)
            {
                user.UserLogins ??= [];
                user.UserLogins.Add(new ThisUserLogin
                {
                    LoginProvider = provider,
                    ProviderKey = providerKey,
                    ProviderDisplayName = ""
                });
                var r = await UserManager.UpdateAsync(user);
                ThrowErrorIfFailed(r);

                var result = MapUserDTO(user);
                return result;
            }
            return null;
        }

        public async Task<UsersOperationResponse> SendResetPasswordEmail(SendResetPasswordEmailRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (string.IsNullOrEmpty(request.UserName))
            {
                throw new ArgumentException(nameof(request.UserName));
            }
            if (string.IsNullOrEmpty(request.ResetPasswordUrl))
            {
                throw new ArgumentException(nameof(request.ResetPasswordUrl));
            }
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User doesn't exist.");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return new UsersOperationResponse("User doesn't have an email");
            }
            //var resetPasswordToken = await CreateResetPasswordToken(user, request.ResetPasswordUrl);
            ////var result = await SendResetPasswordEmail(user.Email, resetPasswordToken);
            //return new UsersOperationResponse(result.Successful, result.ErrorMessages);
            return new UsersOperationResponse(true, string.Empty);
        }

        public async Task<UsersOperationResponse> SetPasswordWithoutToken(SetPasswordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (string.IsNullOrEmpty(request.UserName))
            {
                throw new ArgumentException(nameof(request.UserName));
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException(nameof(request.Password));
            }
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User doesn't exist.");
            }
            var result = await UserManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddPasswordAsync(user, request.Password);
            }
            return new UsersOperationResponse(result.Succeeded, result.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        }

        public async Task<UsersOperationResponse> SetPasswordWithToken(SetPasswordWithTokenRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (string.IsNullOrEmpty(request.UserName))
            {
                throw new ArgumentException(nameof(request.UserName));
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException(nameof(request.Password));
            }
            if (string.IsNullOrEmpty(request.Token))
            {
                throw new ArgumentException(nameof(request.Token));
            }
            var user = await GetUser(request.UserName);
            if (user == null)
            {
                return new UsersOperationResponse("User doesn't exist.");
            }
            var result = await UserManager.ResetPasswordAsync(user, request.Token, request.Password);
            ThrowErrorIfFailed(result);
            // TODO: check if is possible to revoke the current token
            return new UsersOperationResponse(result.Succeeded, result.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        }

        //public async Task<UsersOperationResponse> UpdateClaim(UpdateClaimRequest request)
        //{
        //    var user = await GetUser(request.UserName);
        //    if (user == null)
        //    {
        //        return new UsersOperationResponse("User doesn't exist.");
        //    }
        //    return await AddOrUpdateUserClaim(user, request.ClaimType, request.ClaimValue, false);
        //}

        //public async Task<UsersOperationResponse> UpdateUser(UpdateUserRequest request)
        //{
        //    ArgumentNullException.ThrowIfNull(request);
        //    if (string.IsNullOrEmpty(request.UserName))
        //    {
        //        throw new ArgumentException(nameof(request.UserName));
        //    }
        //    var username = request.UserName;
        //    if (!string.IsNullOrWhiteSpace(request.PreviousUsername))
        //    {
        //        username = request.PreviousUsername;
        //        var newUsernameUser = await GetUser(request.UserName);
        //        if (newUsernameUser != null)
        //        {
        //            return new UsersOperationResponse(false, "New UserName already exists.");
        //        }
        //    }
        //    var user = await GetUser(username);
        //    if (user == null)
        //    {
        //        return new UsersOperationResponse("User doesn't exist.");
        //    }
        //    user.UserName = request.UserName;
        //    user.Email = request.Email;
        //    user.PhoneNumber = request.PhoneNumber;
        //    foreach (var claim in request.Claims)
        //    {
        //        var userClaim = user.GetClaim(claim.Type);
        //        if (userClaim == null)
        //        {
        //            var claimToAdd = new ThisUserClaim()
        //            {
        //                ClaimType = claim.Type,
        //                ClaimValue = claim.Value
        //            };
        //            user.UserClaims.Add(claimToAdd);
        //        }
        //        else
        //        {
        //            userClaim.ClaimValue = claim.Value;
        //        }
        //    }
        //    var updateUserResult = await UserManager.UpdateAsync(user);
        //    ThrowErrorIfFailed(updateUserResult);
        //    return new UsersOperationResponse(updateUserResult.Succeeded, updateUserResult.Errors?.Select(e => e.Description) ?? []);
        //}

        public bool UserNameExists(string userName)
        {
            var exists = UserManager.Users.Any(u => u.UserName == userName);
            return exists;
        }

        public async Task<bool> ValidateCredentials(string? userName, string? password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }
            return await UserManager.CheckPasswordAsync(user, password);
        }

        public async Task<UsersOperationResponse> ValidatePassword(string? password)
        {
            var passwordValidator = new PasswordValidator<ThisUser>();
            var result = await passwordValidator.ValidateAsync(UserManager, new ThisUser(), password);
            return new UsersOperationResponse(result.Succeeded, result.Errors?.Select(m => m.Description));
        }

        #region Private Helpers
        private static UserDTO MapUserDTO(ThisUser user)
        {
            var result = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                FirstName = user.GetClaimValue(JwtClaimTypes.GivenName),
                LastName = user.GetClaimValue(JwtClaimTypes.FamilyName),
                Name = user.GetClaimValue(JwtClaimTypes.Name),
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Claims = [.. user.UserClaims.Select(uc => new ClaimDTO(uc.ClaimType ?? string.Empty, uc.ClaimValue ?? string.Empty))]
            };
            return result;
        }

        private async Task<UsersOperationResponse> AddOrUpdateUserClaim(ThisUser user, string? claimType, string? claimValue, bool insertIfNotExists = true)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }
            if (string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException(nameof(claimValue));
            }
            var userClaim = user.GetClaim(claimType);
            if (userClaim == null)
            {
                if (insertIfNotExists)
                {
                    var claimToAdd = new Claim(claimType, claimValue);
                    var addClaimResult = await UserManager.AddClaimAsync(user, claimToAdd);
                    ThrowErrorIfFailed(addClaimResult);
                    return new UsersOperationResponse(addClaimResult.Succeeded, addClaimResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
                }
                else
                {
                    return new UsersOperationResponse(false, [$"User {user.Id} did not contain any claim of type '{claimType}' and insertIfNotExists is set to false."]);
                }
            }
            else
            {
                if (userClaim.ClaimValue != claimValue)
                {
                    userClaim.ClaimValue = claimValue;
                    var updateUserResult = await UserManager.UpdateAsync(user);
                    ThrowErrorIfFailed(updateUserResult);
                    return new UsersOperationResponse(updateUserResult.Succeeded, updateUserResult.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
                }
                else
                {
                    return new UsersOperationResponse(false, [$"Value for claim '{claimType}' did not change."]);
                }
            }
        }

        private async Task<UsersOperationResponse> RemoveUserClaim(ThisUser user, string? claimType, string? claimValue)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }
            if (string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException(nameof(claimValue));
            }
            var claimToRemove = new Claim(claimType, claimValue);
            var result = await UserManager.RemoveClaimAsync(user, claimToRemove);
            ThrowErrorIfFailed(result);
            return new UsersOperationResponse(result.Succeeded, result.Errors?.Select(e => e.Description) ?? ["Unexpected error"]);
        }

        private async Task<string> CreateResetPasswordToken(ThisUser user, string resetPasswordBaseUrl)
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var uriBuilder = new UriBuilder(resetPasswordBaseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }


        private async Task<ThisUser?> GetUser(string userName)
        {
            var user = await UserManager.Users
                    .Include(u => u.UserClaims)
                    .SingleOrDefaultAsync(m => m.UserName == userName);
            return user;
        }

        //private async Task<SendEmailResponse> SendResetPasswordEmail(string email, string resetPasswordUrl)
        //{
        //    var result = await EmailSender.SendResetPasswordEmail(new EmailRecipient(email), new ResetPasswordEmailModel { ResetPasswordLink = resetPasswordUrl });
        //    return result;
        //}

        private static void ThrowErrorIfFailed(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors?.Select(e => e.Description).FirstOrDefault() ?? "Unexpected error");
            }
        }
        #endregion
    }
}
