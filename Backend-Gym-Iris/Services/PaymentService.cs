using Backend_Gym_Iris.DTOs.Payment;
using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Repositories;

namespace Backend_Gym_Iris.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMembershipPriceRepository _priceRepository;

        public PaymentService(
            IPaymentRepository paymentRepository, 
            IUserRepository userRepository,
            IMembershipPriceRepository priceRepository)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _priceRepository = priceRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _paymentRepository.GetByUserIdAsync(userId);
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<Payment> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new ArgumentException($"Usuario con ID {request.UserId} no encontrado");

            var currentPrice = await _priceRepository.GetByTypeAsync(request.MembershipType);
            if (currentPrice == null)
                throw new InvalidOperationException($"No se encontró precio configurado para {request.MembershipType}");

            if (request.Amount > currentPrice.Price)
                throw new InvalidOperationException(
                    $"El monto ${request.Amount} sobrepasa el precio actual de {request.MembershipType}: ${currentPrice.Price}");

            var exists = await _paymentRepository.ExistsAsync(request.UserId, request.Month, request.Year);
            if (exists)
                throw new InvalidOperationException($"Ya existe un pago para {request.Month}/{request.Year}");

            var payment = new Payment
            {
                UserId = request.UserId,
                MembershipType = request.MembershipType,
                Amount = request.Amount,
                Month = request.Month,
                Year = request.Year,
                Notes = request.Notes,
                PaymentDate = DateTime.UtcNow
            };

            var createdPayment = await _paymentRepository.CreateAsync(payment);

            if (user.Status == Backend_Gym_Iris.Enums.MembershipStatus.Pending)
            {
                user.Status = Backend_Gym_Iris.Enums.MembershipStatus.Active;
                await _userRepository.UpdateAsync(user.Id, user);
            }

            return createdPayment;
        }

        public async Task<Payment?> UpdatePaymentAsync(int id, UpdatePaymentRequest request)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null)
                return null;

            existingPayment.Amount = request.Amount;
            existingPayment.PaymentDate = request.PaymentDate;
            existingPayment.Notes = request.Notes;

            return await _paymentRepository.UpdateAsync(existingPayment);
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            return await _paymentRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetTotalPaidByUserAsync(int userId)
        {
            var payments = await _paymentRepository.GetByUserIdAsync(userId);
            return payments.Sum(p => p.Amount);
        }

        public async Task<bool> HasPaidMonthAsync(int userId, int month, int year)
        {
            return await _paymentRepository.ExistsAsync(userId, month, year);
        }
    }
}
