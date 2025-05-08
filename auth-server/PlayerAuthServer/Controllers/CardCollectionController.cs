using Microsoft.AspNetCore.Mvc;
using PlayerAuthServer.Utilities;
using PlayerAuthServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using PlayerAuthServer.Models.Requests;

namespace PlayerAuthServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/player")]
    public class CardCollectionController(ICardCollectionService cardCollectionService) : ControllerBase
    {
        [HttpPost("collection")]
        public async Task<IActionResult> PostCardCollection([FromBody] CollectCardRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("Id")?.Value;
                if (Guid.TryParse(userIdClaim, out var playerId))
                {
                    Logger.Info($"{playerId}");
                    var cardCollection = await cardCollectionService.CollectCard(request, playerId);
                    return Ok(cardCollection);
                }
                else return Unauthorized();
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex.Message);
                return StatusCode(500, "Unexpected error has occurred");
            }
        }

        [HttpPost("collection/check")]
        public async Task<IActionResult> CheckCardCollection([FromBody] CheckCardCollectionRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("Id")?.Value;
                if (Guid.TryParse(userIdClaim, out var playerId))
                {
                    var cardCollection = await cardCollectionService.CheckCollection(request, playerId);
                    return Ok(cardCollection);
                }

                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex.Message);
                return StatusCode(500, "Unexpected error has occurred");
            }
        }
    }
}
