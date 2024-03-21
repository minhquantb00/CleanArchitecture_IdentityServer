using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse
{
    public class TokenType
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryTokenDate { get; set; }
    }
}
