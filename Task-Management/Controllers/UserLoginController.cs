using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_Management.Database;
using Task_Management.DTOs.ResponseDto;
using Task_Management.Models;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {

        private readonly TaskContext _context;
        private readonly IConfiguration _configuration;

        public UserLoginController(TaskContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            try
            {
                var userReq = new UserLogin
                {
                    FullName = userModel.FullName,
                    Email = userModel.Email,
                    Role = userModel.Role,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password)
                };

              
                var data = await _context.UserLogins.AddAsync(userReq);
                await _context.SaveChangesAsync();
                var res = new UserModel
                {
                    Id = data.Entity.Id,
                    FullName = data.Entity.FullName,
                    Email = data.Entity.Email,
                    Role = data.Entity.Role,
                 
                };
                var token = CreateToken(res);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("auth/login")]

        public async Task<IActionResult>Login(string Email, string password)
        {
            try
            {
                var user =await _context.UserLogins.FirstOrDefaultAsync(x => x.Email == Email);
                if (user == null) 
                {
                    throw new Exception("User Not Found");
                }
                var IsValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                return Ok(IsValid);
            }
            catch (Exception ex)
            {
                return BadRequest (ex);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<string> Test()
        {
            var data = User.FindFirst("Roles").Value;

            return data;
        }

        protected TokenModel CreateToken(UserModel UserModel)
        {
            var claimList = new List<Claim>();
            claimList.Add(new Claim("Id", UserModel.Id.ToString()));
            claimList.Add(new Claim("FullName", UserModel.FullName));
            claimList.Add(new Claim("Email", UserModel.Email));
            claimList.Add(new Claim("Role", UserModel.Role.ToString()));

            var Key = _configuration["Jwt:Key"];
            var secKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var togen = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claimList,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
                );

            var res = new TokenModel();
            res.Token = new JwtSecurityTokenHandler().WriteToken(togen);
            return res;

        }

    }
}
