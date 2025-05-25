using Microsoft.AspNetCore.Mvc;
using PlayerAuthServer.Utilities;
using PlayerAuthServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using PlayerAuthServer.Models.Requests;
using PlayerAuthServer.Models.Responses;

namespace PlayerAuthServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/player/collection")]
    public class CardCollectionController(ICardCollectionService cardCollectionService, ICardCollectionRepository cardCollectionRepository) : ControllerBase
    {
        [Authorize]
        [HttpPost("/")]
        public async Task<IActionResult> PostCardCollection([FromBody] CollectCardRequest request)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (Guid.TryParse(userIdClaim, out var playerId))
            {
                Logger.Info($"{playerId}");
                var cardCollection = await cardCollectionService.CollectCard(request, playerId);
                return Ok(cardCollection);
            }

            return Unauthorized();
        }

        
        [Authorize]
        [HttpGet("/")]
        public async Task<IActionResult> GetCardCollection()
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (Guid.TryParse(userIdClaim, out var playerId))
            {
                Logger.Info($"{playerId}");
                var cardCollection = await cardCollectionRepository.FindOwnedCards(playerId);
                return Ok(new GetCardCollectionResponse(cardCollection.Count, cardCollection));
            }

            return Unauthorized();
        }
        
        [HttpPost("verify")]
        public async Task<IActionResult> CheckCardCollection([FromBody] CheckCardCollectionRequest request)
        {
            var userIdClaim = User.FindFirst("Id")?.Value;
            if (Guid.TryParse(userIdClaim, out var playerId))
            {
                var cardCollection = await cardCollectionService.CheckCollection(request, playerId);
                return Ok(cardCollection);
            }

            return Unauthorized();
        }
    }
}