

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Data;
using WebApiDemo.Models;
using WebApiDemo.Models.Registration;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationApiDbContext _dbContext;

        public RegistrationController(RegistrationApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AddRegistrationRequest addRegistrationRequest)
        {
            // Check if the username already exists
            if (await _dbContext.Registrations.AnyAsync(r => r.UserName == addRegistrationRequest.UserName))
            {
                return BadRequest("Username already exists");
            }

            var registration = new Registration
            {
                UserName = addRegistrationRequest.UserName,
                Email = addRegistrationRequest.Email,
                FullName = addRegistrationRequest.FullName,
                Password = addRegistrationRequest.Password
            };

            _dbContext.Registrations.Add(registration);
            await _dbContext.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var registration = await _dbContext.Registrations.FirstOrDefaultAsync(r => r.UserName == loginRequest.UserName && r.Password == loginRequest.Password);

            if (registration == null)
            {
                return Unauthorized("Invalid username or password");
            }
            ///////
           
            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            ///code sekret key ////
            byte[] bytes = new byte[32]; // 32 bytes = 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            string secretKey = Convert.ToBase64String(bytes);
            //end secrect key//

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, registration.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Return token along with the registration data
            return Ok(new { Token = tokenString, Registration = registration });
        }
    }
}