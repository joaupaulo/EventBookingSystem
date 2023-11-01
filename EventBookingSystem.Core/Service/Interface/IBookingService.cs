using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IBookingService
{
    Task<Booking> CreateReserva(Booking reserva);
    Task<List<Booking>> GetAllReservas();
    Task<Booking> GetReserva(Guid reservaKey);
    Task<bool> DeleteReserva(Guid reservaKey);
    Task<bool> UpdateReserva(string filterDefinitionField, string filterDefinitionParam, string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);
}