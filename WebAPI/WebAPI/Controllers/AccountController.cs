using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.ApiErrors;
using WebAPI.BL.Interface;
using WebAPI.DAL.Database;
using WebAPI.DAL.Entities;
using WebAPI.Dtos;
using WebAPI.Migrations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(IUnitOfWork uow, UserManager<IdentityUser> userManager)
        {           
            _uow = uow;
            _userManager = userManager;
            
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto registerDTO)
        {

            var appUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (appUser != null)
                return BadRequest("This Email is already exists");

            appUser = new()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                EmailConfirmed = true
            };
            IdentityResult result =
                await 
                _userManager.CreateAsync(appUser, registerDTO.Password);

            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);


            if (user != null && _userManager.CheckPasswordAsync(user, loginDTO.Password).Result)
                return Ok(await _uow.JWTService.GenerateToken(user));

            return BadRequest("Invalid account");

        }
    }




    }


