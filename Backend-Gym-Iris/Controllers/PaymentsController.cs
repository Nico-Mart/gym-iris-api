using Backend_Gym_Iris.DTOs.Payment;
using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Gym_Iris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Obtiene todos los pagos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            var response = payments.Select(p => new PaymentResponse(
                p.Id,
                p.UserId,
                p.User.Name,
                p.MembershipType.ToString(),
                p.Amount,
                p.PaymentDate,
                p.Month,
                p.Year,
                p.Notes
            ));
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un pago por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponse>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                return NotFound(new { message = $"Pago con ID {id} no encontrado" });
            }

            var response = new PaymentResponse(
                payment.Id,
                payment.UserId,
                payment.User.Name,
                payment.MembershipType.ToString(),
                payment.Amount,
                payment.PaymentDate,
                payment.Month,
                payment.Year,
                payment.Notes
            );

            return Ok(response);
        }

        /// <summary>
        /// Obtiene todos los pagos de un usuario
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetPaymentsByUser(int userId)
        {
            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId);
            var response = payments.Select(p => new PaymentResponse(
                p.Id,
                p.UserId,
                p.User.Name,
                p.MembershipType.ToString(),
                p.Amount,
                p.PaymentDate,
                p.Month,
                p.Year,
                p.Notes
            ));
            return Ok(response);
        }

        /// <summary>
        /// Obtiene el total pagado por un usuario
        /// </summary>
        [HttpGet("user/{userId}/total")]
        public async Task<ActionResult<object>> GetTotalPaidByUser(int userId)
        {
            var total = await _paymentService.GetTotalPaidByUserAsync(userId);
            return Ok(new { userId, totalPaid = total });
        }

        /// <summary>
        /// Verifica si un usuario pagó un mes específico
        /// </summary>
        [HttpGet("user/{userId}/check/{year}/{month}")]
        public async Task<ActionResult<object>> CheckMonthPaid(int userId, int year, int month)
        {
            var hasPaid = await _paymentService.HasPaidMonthAsync(userId, month, year);
            return Ok(new { userId, year, month, hasPaid });
        }

        /// <summary>
        /// Crea un nuevo pago
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Month < 1 || request.Month > 12)
            {
                return BadRequest(new { message = "El mes debe estar entre 1 y 12" });
            }

            if (request.Amount <= 0)
            {
                return BadRequest(new { message = "El monto debe ser mayor a 0" });
            }

            try
            {
                var createdPayment = await _paymentService.CreatePaymentAsync(request);

                var response = new PaymentResponse(
                    createdPayment.Id,
                    createdPayment.UserId,
                    createdPayment.User.Name,
                    createdPayment.MembershipType.ToString(),
                    createdPayment.Amount,
                    createdPayment.PaymentDate,
                    createdPayment.Month,
                    createdPayment.Year,
                    createdPayment.Notes
                );

                return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un pago existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentRequest request)
        {
            var updatedPayment = await _paymentService.UpdatePaymentAsync(id, request);

            if (updatedPayment == null)
            {
                return NotFound(new { message = $"Pago con ID {id} no encontrado" });
            }

            var response = new PaymentResponse(
                updatedPayment.Id,
                updatedPayment.UserId,
                updatedPayment.User?.Name ?? "Usuario", 
                updatedPayment.MembershipType.ToString(),
                updatedPayment.Amount,
                updatedPayment.PaymentDate,
                updatedPayment.Month,
                updatedPayment.Year,
                updatedPayment.Notes
            );

            return Ok(response);
        }

        /// <summary>
        /// Elimina un pago
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var result = await _paymentService.DeletePaymentAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Pago con ID {id} no encontrado" });
            }

            return NoContent();
        }
    }
}
