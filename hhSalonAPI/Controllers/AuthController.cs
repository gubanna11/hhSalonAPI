using hhSalonAPI.Data.Static;
using hhSalonAPI.Data;
using hhSalonAPI.Data.Models;
using hhSalonAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace hhSalonAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		public AuthController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] User userObj)
		{
			if (userObj == null)
			{
				return BadRequest();
			}

			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.UserName == userObj.UserName);

			if (user == null)
				return NotFound(new { Message = "User not found!" });
			
			if(!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
				return BadRequest(new { Message = "Incorrect password!" });

			return Ok(new { Message = "Login success!" });
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] User userObj)
		{
			if (userObj == null)
			{
				return BadRequest();
			}
		

			//double id
			if(_context.Users.Where(u => u.Id == userObj.Id).Count() > 0)
				while(_context.Users.Where(u => u.Id == userObj.Id).Count() != 0)
					userObj.Id = Guid.NewGuid().ToString();


			//double name
			if (await CheckUserNameExistAsync(userObj.UserName))
				return BadRequest(new { Message = "This UserName is already taken!" });

			//double email

			if (await CheckEmailExistAsync(userObj.Email))
				return BadRequest(new { Message = "This Email is already taken!" });

			//check password

			var pass = CheckPasswordStrength(userObj.Password);
			if(!string.IsNullOrEmpty(pass))
				return BadRequest(new { Message = pass });


			userObj.Password = PasswordHasher.HashPassword(userObj.Password);
			userObj.Role = UserRoles.Client;

			userObj.Token = "";

			userObj.Id = Guid.NewGuid().ToString();


			await _context.Users.AddAsync(userObj);
			await _context.SaveChangesAsync();

			//return Ok(_context.Users.ToList());
			return Ok(new
			{
				Message = "User Registered!"
			});
		}

		private async Task<bool> CheckUserNameExistAsync(string userName)
			=> await _context.Users.AnyAsync(u => u.UserName == userName);


		private async Task<bool> CheckEmailExistAsync(string email)
			=> await _context.Users.AnyAsync(u => u.Email == email);

		private string CheckPasswordStrength(string password)
		{
			StringBuilder stringBuilder= new StringBuilder();

			if(password.Length < 8)
				stringBuilder.Append("Minimum password length should be 8" + Environment.NewLine);

			if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") 
				&& Regex.IsMatch(password, "[0-9]")))
					stringBuilder.Append("Password should be Alphanumeric" + Environment.NewLine);

			if(!(Regex.IsMatch(password, @"[<,>@!#$%^&*()_+\[\]{}?:;|'./~\-=]")))
				stringBuilder.Append("Password should contain special characters" + Environment.NewLine);

			return stringBuilder.ToString();
		}
	}
}
