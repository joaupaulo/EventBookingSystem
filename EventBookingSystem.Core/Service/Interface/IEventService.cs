using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IEventService
{
    Task<Evento> CreateEvent(Evento evento);
    Task<Evento> GetEvents(Guid eventKey);
    Task<bool> DeleteEvent(string eventKey);
    Task<List<Evento>> GetAllEvents();

    Task<bool> UpdateEvent<T>(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);

}