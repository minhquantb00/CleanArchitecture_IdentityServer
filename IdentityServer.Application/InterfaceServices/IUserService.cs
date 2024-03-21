using IdentityServer.Application.ImplementServices.UseCases.UserCase;
using IdentityServer.Application.ImplementServices.UseCases.UserCase.Requests;
using IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse;
using IdentityServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application.InterfaceServices
{
    public interface IUserService
    {
        Task<ApiResponse<DataCreateUserResponse>> CreateUserWithTokenAsync(Request_Register registerUser);
        Task<ApiResponse<List<string>>> AssignRoleToUserAsync(List<string> roles, ApplicationUser user);
        Task<ApiResponse<LoginOtpResponse>> GetOtpByLoginAsync(Request_Login loginModel);
        Task<ApiResponse<DataLoginResponse>> GetJwtTokenAsync(ApplicationUser user);
        Task<ApiResponse<DataLoginResponse>> LoginUserWithJWTokenAsync(string otp, string userName);
        Task<ApiResponse<DataLoginResponse>> RenewAccessTokenAsync(DataLoginResponse tokens);
    }
}
