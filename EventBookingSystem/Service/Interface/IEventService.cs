using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;

namespace EventBookingSystem.Service.Interface;

public interface IEventService
{
    Task<Evento> CreateEvent(EventoDto evento);
    Task<EventoDto> GetEvents<EventoDto>(string eventKey);
    Task<bool> DeleteEvent<EventoDto>(string eventKey);
    Task<List<EventoDto>> GetAllEvents<EventoDto>();

    Task<bool> UpdateEvent<T>(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);

}