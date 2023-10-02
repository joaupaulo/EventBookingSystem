using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IReservaService
{
    Task<EventoReservado> CreateReserva(EventoReservado reserva);
    Task<List<EventoReservado>> GetAllReservas();
    Task<EventoReservado> GetReserva(string reservaKey);
    Task<bool> DeleteReserva(string reservaKey);
    Task<bool> UpdateReserva(EventoReservado reserva, string reservaKey);
}