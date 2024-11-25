using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stablemint.Data;
using stablemint.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace stablemint.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShortLinkController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShortLinkController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var links = await _context.ShortLinks
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return Ok(links);
        }

        [HttpPost]
        public async Task<ActionResult<ShortLink>> Create([FromBody] CreateShortLinkModel request)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            var recentLinksCount = await _context.ShortLinks
                .Where(l => l.UserId == userId && l.CreatedAt >= oneHourAgo)
                .CountAsync();

            if (recentLinksCount >= 10)
            {
                return StatusCode(429, "Rate limit exceeded. Please try again later.");
            }

            var shortLink = new ShortLink
            {
                ShortId = Guid.NewGuid().ToString()[..6],
                LongUrl = request.Url,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                Clicks = 0
            };

            _context.ShortLinks.Add(shortLink);
            await _context.SaveChangesAsync();

            return Ok(shortLink);
        }

        [HttpGet("/link/{shortId}")]
        [AllowAnonymous]
        public async Task<ActionResult> RedirectMethod(string shortId)
        {
            var shortLink = await _context.ShortLinks
                .FirstOrDefaultAsync(l => l.ShortId == shortId);

            if (shortLink == null)
            {
                return NotFound();
            }

            shortLink.Clicks++;
            await _context.SaveChangesAsync();

            return Redirect(shortLink.LongUrl);
        }
    }
}