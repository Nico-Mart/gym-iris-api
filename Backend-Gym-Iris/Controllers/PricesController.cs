using Backend_Gym_Iris.DTOs.MembershipPrice;
using Backend_Gym_Iris.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Gym_Iris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IMembershipPriceService _priceService;

        public PricesController(IMembershipPriceService priceService)
        {
            _priceService = priceService;
        }

        /// <summary>
        /// Obtiene todos los precios de membresías
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceResponse>>> GetAllPrices()
        {
            var prices = await _priceService.GetAllPricesAsync();
            var response = prices.Select(p => new PriceResponse(
                p.Id,
                p.Type.ToString(),
                p.Price,
                p.LastUpdated
            ));
            return Ok(response);
        }

        /// <summary>
        /// Obtiene el precio de un tipo específico de membresía
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceResponse>> GetPrice(int id)
        {
            var price = await _priceService.GetPriceByIdAsync(id);

            if (price == null)
            {
                return NotFound(new { message = $"Precio con ID {id} no encontrado" });
            }

            var response = new PriceResponse(
                price.Id,
                price.Type.ToString(),
                price.Price,
                price.LastUpdated
            );

            return Ok(response);
        }

        /// <summary>
        /// Actualiza el precio de una membresía (solo Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PriceResponse>> UpdatePrice(int id, [FromBody] UpdatePriceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Price <= 0)
            {
                return BadRequest(new { message = "El precio debe ser mayor a 0" });
            }

            try
            {
                var updatedPrice = await _priceService.UpdatePriceAsync(id, request.Price);

                var response = new PriceResponse(
                    updatedPrice.Id,
                    updatedPrice.Type.ToString(),
                    updatedPrice.Price,
                    updatedPrice.LastUpdated
                );

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
