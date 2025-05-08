using Microsoft.AspNetCore.Mvc;
using DeckCollection.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DeckCollection.Models;

namespace DeckCollection.Core
{
    [ApiController]
    [Route("api/deck")]
    public class DeckController(IDeckService deckService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPlayerDecks()
        {
            try
            {
                var idClaim = User.FindFirst("id")?.Value;
                if (idClaim == null) return Unauthorized("Could not get id claim");

                if (Guid.TryParse(idClaim, out var playerId))
                {
                    var insertDeck = await deckService.GetPlayerDecksAsync(playerId);
                    return Ok(insertDeck);
                }

                return Unauthorized("Player ID invalid");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, "Unexpected error occurred");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostPlayerDeck([FromBody] CreateDeckRequest request)
        {
            try
            {
                var idClaim = User.FindFirst("id")?.Value;
                if (idClaim == null) return Unauthorized("Could not get id claim");

                if (Guid.TryParse(idClaim, out var playerId))
                {
                    var insertDeck = await deckService.CreateDeckAsync(request, playerId);
                    return Ok(insertDeck);
                }

                return Unauthorized("Player ID invalid");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, "Unexpected error occurred");
            }
        }

        [Authorize]
        [HttpGet("{deckId}")]
        public async Task<IActionResult> GetPlayerDeckById(string deckId)
        {
            try
            {
                var idClaim = User.FindFirst("id")?.Value;
                if (idClaim == null) return Unauthorized("Could not get id claim");

                if (Guid.TryParse(idClaim, out var playerId))
                {
                    if (Guid.TryParse(deckId, out var deckUuid))
                    {
                        var deck = await deckService.GetDeckAsync(playerId, deckUuid);
                        return Ok(deck);
                    }
                    else return BadRequest("Invalid Deck UUID");
                }
                else return Unauthorized("Player ID invalid");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, "Unexpected error occurred");
            }
        }
    }
}
