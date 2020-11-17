using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Data.Entities;
using EFConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EFConnect.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService authService, IConfiguration config)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            if (!string.IsNullOrEmpty(userForRegister.Username))
                userForRegister.Username = userForRegister.Username.ToLower();

            if (await _authService.UserExists(userForRegister.Username))
                ModelState.AddModelError("Username", "Username already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var createUser = await _authService.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLogin userForLogin)
        {
            var userFromDb = await _authService.Login(userForLogin.Username.ToLower(), userForLogin.Password);

            if (userFromDb == null)
                return Unauthorized();

            //  GENERATE TOKEN
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);   // <---- Added                              // we'll add this after adding DI later
            var tokenDescriptor = new SecurityTokenDescriptor    //  Describes information we want to include in our token
            {
                Subject = new ClaimsIdentity(new Claim[]        //  Payload
                {
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
            new Claim(ClaimTypes.Name, userFromDb.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);  //  Create token
            var tokenString = tokenHandler.WriteToken(token);       //  to string (from byte[])

            return Ok(new { tokenString });                        //  Return 200, passing along tokenString
        }
    }
}