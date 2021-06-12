using dotnet_project.DTO;
using dotnet_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.APIController
{
    [Route("api/register")]
    [ApiController]
    public class RegisterControllers : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterControllers(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Register([FromForm] UserDTO dto)
        {
            var user = new ApplicationUser();
            user.Email = dto.Email;
            await _userManager.CreateAsync(user, dto.Password);

            return Ok(new
            {
                status = "success"
            });
        }
    }
}
