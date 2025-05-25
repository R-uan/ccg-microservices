using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayerAuthServer.Interfaces;
using PlayerAuthServer.Models;

namespace PlayerAuthServer.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController(IPlayerService playerService, IPlayerRepository playerRepository) : ControllerBase
    {
        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPartialPlayerProfileProfile(Guid playerId)
        {
            var player = await playerRepository.FindPlayer(playerId);
            return player != null ? Ok(PartialPlayerProfile.Create(player)) : NotFound();
        }

        [Authorize]
        [HttpGet("account")]
        public async Task<IActionResult> GetPlayerProfile()
        {
            var idClaim = User.FindFirst("Id")?.Value;
            if (Guid.TryParse(idClaim, out var playerId))
            {
                var player = await playerRepository.FindPlayer(playerId);
                return player != null ? Ok(PlayerProfile.Create(player)) : NotFound();
            }

            return Unauthorized();
        }
    }
}