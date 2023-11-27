using BusinessLogic.Interfaces;
using Common;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        ICardService _cardService;
        IMapper _mapper;
        ILogger<CardController> _logger;
        public CardController(IOptions<AuthOptions> authOptions, ICardService cardService,
            IMapper mapper, ILogger<CardController> logger)
        {
            AuthOptions = authOptions;
            this._cardService = cardService;
            this._mapper = mapper;
            _logger = logger;
        }

        public IOptions<AuthOptions> AuthOptions { get; }

        /// <summary>
        /// Get list of all cards for user
        /// </summary>
        /// <returns>Returns IEnumerable<CardDto></CardDto></returns>
        [HttpGet(Name = "GetAllCards")]
        [Authorize("MustBeOrderListOwner")]
        public ActionResult<IEnumerable<CardDto>> GetAll(int userId)
        {
                var cards = _cardService.GetAll(userId);
                return Ok(_mapper.Map<IEnumerable<CardDto>>(cards));
        }

        /// <summary>
        /// Get card by id
        /// </summary>
        /// <param name="id">The id of the card to get</param>
        /// <returns>ActionResult<CardDTO>></returns>
        /// <response code = "200">Returns the requested card</response>
        [HttpGet("{id}", Name = "GetCard")]
        [Authorize("MustBeCardOwner")]
        public async Task<ActionResult<CardDto>> Get(int id)
        {
            try
            {
                var card = await _cardService.Get(id);

                if (card == null)
                    return NotFound($"Card with id: {id} doesn`t exists.");

                return Ok(_mapper.Map<CardDto>(card));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogCritical(
                    $"Exception while hetting card by id {id}.");

                return StatusCode(500, $"A problem happened while handling your request.{ex.Message}");
            }
        }

        /// <summary>
        /// Add a new card for user
        /// </summary>
        /// <param name="card">New card</param>
        /// <returns>returns IActionResult</returns>
        [Route("signup")]
        [HttpPost]
        [Authorize("MustBeCardOwner")]
        public async Task<IActionResult> Create([FromBody] CardCreateDto card)
        {
            try
            {
                if (card == null)
                    return BadRequest();

                var cardToCreate = _mapper.Map<Card>(card);
                await _cardService.Create(cardToCreate);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update card
        /// </summary>
        /// <param name="id">The id of the card to update</param>
        /// <param name="updatedCard">balance</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize("MustBeCardOwner")]
        public async Task<IActionResult> Update(int id, [FromBody] CardUpdateDto updatedCard)
        {
            if (updatedCard == null)
                return BadRequest();

            var card = await _cardService.Get(id);

            if (card == null)
                return NotFound($"Card with id: {id} doesn`t exists.");

            var cardToUpdate = _mapper.Map<Card>(updatedCard);
            await _cardService.Update(id, cardToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Delete card by id
        /// </summary>
        /// <param name="id">The id of card to delete</param>
        /// <returns>Returns deleted card</returns>
        [HttpDelete("{id}")]
        [Authorize("MustBeCardOwner")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedCard = await _cardService.Delete(id);

            if (deletedCard == null)
                return NotFound();

            _logger.LogInformation($"Card with id {id} was successfully deleted.");
            return new ObjectResult(deletedCard);
        }
    }
}
