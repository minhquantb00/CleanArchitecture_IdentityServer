using IdentityServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse
{
    public class DataCreateUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}
