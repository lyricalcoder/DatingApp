using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingAppAPI.Data;
using DatingAppAPI.Dtos;
using DatingAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using DatingAppAPI.Security;

namespace DatingAppAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO regUserDto)
        {
            regUserDto.UserName = regUserDto.UserName.ToLower();

            if (await _repo.UserExists(regUserDto.UserName))
                return BadRequest("That User Name already exists.");

            var newUser = new User
            {
                UserName = regUserDto.UserName
            };

            var createdUser = await _repo.Register(newUser, regUserDto.Password);
            return StatusCode(201); // Fix this later to CreatedAtRoute
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDTO loginUserDto)
        {
            var userFromRepo = await _repo.Login(loginUserDto.UserName.ToLower(), loginUserDto.Password);
            if (userFromRepo == null)
                return Unauthorized();

            // If the user is valid, create a JSON Web Token (JWT) with
            // the user's ID and UserName
            var tokenDescriptor = TokenUtility.CreateTokenDescriptor(userFromRepo, _config);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token as an object to the user in the response.
            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}