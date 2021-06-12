using dotnet_project.DTO;
using dotnet_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_project.APIController
{
    [Route("api/authen")]
    [ApiController]
    public class LoginContollers : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginContollers(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> GetToken([FromBody] UserDTO dto)
        {
            var claimList = new List<Claim>();

            var user = await _userManager.FindByNameAsync(dto.Email);
            if(await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                claimList.Add(new Claim(ClaimTypes.Name, user.UserName));
                claimList.Add(new Claim("id", user.Id));
            }
            else
            {
                BadRequest("Wrong email or password");
            }
            

            var userClaims = await _userManager.GetClaimsAsync(user);
            claimList.Concat(userClaims);

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, role));
            }
            var issuer = "hak";

            var key = "hak123456789hak123456789";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: issuer,
                claims: claimList,
                signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return Ok(new
            {
                token = tokenString
            });
        }
    }
}
