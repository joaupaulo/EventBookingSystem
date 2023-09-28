using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;

namespace EventBookingSystem.Service.Interface;

public interface IEventService
{
    Task<EventoDto> CreateEvent<EventoDto>(EventoDto evento);
}