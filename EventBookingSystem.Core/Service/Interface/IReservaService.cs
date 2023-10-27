using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IReservaService
{
    Task<Reserva> CreateReserva(Reserva reserva);
    Task<List<Reserva>> GetAllReservas();
    Task<Reserva> GetReserva(string reservaKey);
    Task<bool> DeleteReserva(string reservaKey);
    Task<bool> UpdateReserva(string filterDefinitionField, string filterDefinitionParam, string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);
}