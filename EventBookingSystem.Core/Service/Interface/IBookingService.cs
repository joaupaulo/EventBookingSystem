using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IBookingService
{
    Task<Booking> CreateBooking(Booking reserva);
    Task<List<Booking>> GetAllBooking();
    Task<Booking> GetBooking(Guid reservaKey);
    Task<bool> DeleteBooking(Guid reservaKey);
    Task<bool> UpdateBooking(string filterDefinitionField, string filterDefinitionParam, string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);
}