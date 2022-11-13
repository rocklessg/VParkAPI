using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos;
using VPark_Models.Dtos.AccountDto;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountRepository> _logger;
        private readonly IConfiguration _configuration;
        public AccountRepository(UserManager<IdentityUser> userManager, ILogger<AccountRepository> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;           
            _configuration = configuration;
        }

        public async Task<Response<IdentityResult>> Register(UserRegisterationDto register)
        {
            _logger.LogInformation(message: "Attempting to create a new user", register);
            var user = new AppUser
            {
                UserName = register.EmailAddress,
                Email = register.EmailAddress,
                PhoneNumber= register.PhoneNumber,  
                FirstName = register.FirstName,
                LastName = register.LastName,
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            _logger.LogInformation(message: $"new user with UserName:{register.EmailAddress} created", register);

            if (!result.Succeeded)
            {
                return new Response<IdentityResult>()
                {
                    Succeeded = false,
                    Message = "Registeration Failed, please retry"
                };
            }
            return new Response<IdentityResult>()
            {
                Succeeded = true,
                Message = "User Registered Successfully",
            };

           
           
        }
        

        public async Task<Response<IdentityResult>> Login(UserLoginDto login)
        {
            _logger.LogInformation(message: $"Attempt to login by User with Email: {login.Email}", login);
            IdentityUser user =await _userManager.FindByEmailAsync(login.Email);
            _logger.LogInformation(message: $"Email Address : {login.Email} exists in the Database", login);
            if (user == null)
            {
                return new Response<IdentityResult>()
                {
                    Succeeded = false,
                    Message = $"There is no User with this Email: {login.Email} "
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!result)
            {
                return new Response<IdentityResult>()
                {
                    Succeeded = false,
                    Message = $"Invalid Password, Please re-enter your password correctly"
                };            
            }
            var claims = new[]
            {
                new Claim("Email", login.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id )
            };

            //Encrypt the token
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY"))); 
            //var key = Environment.GetEnvironmentVariable("KEY");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
               // audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response<IdentityResult>()
            {
                Succeeded = true,
                Message = tokenAsString,
                ExpireDate = token.ValidTo
                
            };

        }
    }
}
