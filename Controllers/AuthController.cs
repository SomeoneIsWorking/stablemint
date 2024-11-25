using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stablemint.Data;
using stablemint.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace stablemint.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NoContent();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            if (user == null)
            {
                return NoContent();
            }
            return Ok(user.Email);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            return Ok(user.Email);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok();
        }

    }
}