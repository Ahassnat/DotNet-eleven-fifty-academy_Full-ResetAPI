using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EFConnect.Contracts;
using EFConnect.Data.Entities;
using EFConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFConnect.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            return Ok(user);
        }
        [HttpPut("{id}")]
     //   [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdate userForUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
           var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            
            var userFromRepo = await _userService.GetUser(id);

            if (userFromRepo == null)
                return NotFound($"User could not be found.");

            if (currentUserId != userFromRepo.Id)
                return Unauthorized();

            if (await _userService.UpdateUser(id, userForUpdate))
                return NoContent();

            throw new Exception($"Updating user failed on save.");
        }


    }
}