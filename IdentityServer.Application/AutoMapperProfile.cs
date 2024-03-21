using AutoMapper;
using IdentityServer.Application.ImplementServices.UseCases.UserCase.Responses.LoginResponse;
using IdentityServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application
{
    public class AutoMapperProfile : Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, DataCreateUserResponse>();
        }
    }
}
