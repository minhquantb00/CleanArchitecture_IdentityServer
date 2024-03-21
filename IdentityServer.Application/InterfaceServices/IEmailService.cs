using IdentityServer.Application.ImplementServices.UseCases.UserCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application.InterfaceServices
{
    public interface IEmailService
    {
        string SendEmail(Message message);
    }
}
