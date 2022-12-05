using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos;
using VPark_Models.Dtos.AccountDto;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Response<IdentityResult>> Register(UserRegisterationDto register);
        Task<Response<IdentityResult>> Login(UserLoginDto login);
    }
}
