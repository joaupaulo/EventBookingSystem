using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface;

public interface IEventService
{
    Task<Event> CreateEvent(Event evento);
    Task<Event> GetEvents(Guid eventKey);
    Task<bool> DeleteEvent(Guid eventKey);
    Task<List<Event>> GetAllEvents();

    Task<bool> UpdateEvent<T>(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan);

}