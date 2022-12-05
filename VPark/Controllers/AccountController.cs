using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos.AccountDto;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _acctRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository acctRepo, ILogger<AccountController> logger)
        {
            _acctRepo = acctRepo;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterationDto register)
        {
            var result = await _acctRepo.Register(register);
            if (!result.Succeeded)
            {
               return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto login)
        {
            if (login == null)
            {
                return BadRequest("please Enter your login details");
            }

            var result = await _acctRepo.Login(login);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
