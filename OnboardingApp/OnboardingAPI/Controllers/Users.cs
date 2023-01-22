using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnboardingAPI.Resources;
using OnboardingApp.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnboardingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<User> uManager;
        private readonly SignInManager<User> sManager;
        private readonly RoleManager<IdentityRole<int>> rManager;
        private readonly IConfiguration Configuration;

        public Users(UserManager<User> UManager, 
                     SignInManager<User> SManager,
                     RoleManager<IdentityRole<int>> RManager,
                     IConfiguration config)
        {
            uManager = UManager;
            sManager = SManager;
            rManager = RManager;
            Configuration = config;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserSignUp model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await uManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await uManager.AddToRoleAsync(user, model.Role);
                    return Ok(new LoginResponse
                    {
                        ErrorMessage = "Created User with Sucess"
                    });
                }
                else
                {
                    return Ok(new LoginResponse
                    {
                        ErrorMessage = "Could not create user"
                    });
                }
            }
            else
            {
                return Ok();
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {

            var result = await sManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if (result.Succeeded)
            {
                var user = await uManager.FindByEmailAsync(model.Email);

                var userRoles = await uManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    };

                foreach(var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(Configuration["Token:Issuer"],
                                                Configuration["Token:Audience"],
                                                claims,
                                                signingCredentials: creds,
                                                expires: DateTime.UtcNow.AddMinutes(20));
                return Ok(new LoginResponse
                {  
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    IsAuthSuccessful = true,
                    Role = userRoles.FirstOrDefault(),
                    Username = user.Name,
                    UserId = user.Id.ToString()
                });
            }
            else
            {
                return Unauthorized(new LoginResponse
                {
                    ErrorMessage = "Wrong login information",
                    IsAuthSuccessful = false
                });
            }
            
        }

    }
}
