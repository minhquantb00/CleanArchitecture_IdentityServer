using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse
{
    public class DataLoginResponse
    {
        public TokenType AccessToken { get; set; }
        public TokenType RefreshToken { get; set; }
    }
}
