using CardCatalog.Interface;
using CardCatalog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardCatalog.Core
{
    [ApiController]
    [Route("api/cards")]
    public class CatalogController(ICardCatalogRepository cardCatalogRepository, ICardCatalogService cardCatalogService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostMany(PostManyCardsRequest request)
        {
            try
            {
                var save = await cardCatalogRepository.SaveMultipleCards(request.Cards);
                return Ok(save);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[Error] {ex.Message}");
                return StatusCode(500, "Unexpected error occurred");
            }
        }

        [HttpGet("catalog")]
        public async Task<IActionResult> GetCatalog()
        {
            try
            {
                var catalog = await cardCatalogRepository.FindCards();
                return Ok(catalog);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[Error] {ex.Message}");
                return StatusCode(500, "Unexpected error occurred");
            }
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetOneCard(string guid)
        {
            try
            {
                if (Guid.TryParse(guid, out var cardId))
                {
                    var card = await cardCatalogRepository.FindCard(cardId);
                    return card != null ? Ok(card) : NotFound(cardId);
                }
                else return BadRequest("Invalid guid");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[Error] {ex.Message}");
                return StatusCode(500, "Unexpected error occurred");
            }
        }

        [HttpPost("select")]
        public async Task<IActionResult> GetCards([FromBody] SelectedCardsRequest request)
        {
            try
            {
                var response = await cardCatalogService.SelectedCards(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[Error] {ex.Message}");
                return StatusCode(500, "Unexpected Error has occured");
            }
        }
    }
}
