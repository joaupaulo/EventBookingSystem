using EventBookingSystem.BsonFilter;
using EventBookingSystem.Model;
using EventBookingSystem.Repository;
using EventBookingSystem.Service.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EventBookingSystem.Service;

public class EventService : RepositoryBase, IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private string _collectionName = "event-collection";
    private readonly IBsonFilter<Event> _bsonFilter;

    public EventService(IRepositoryBase repositoryBase, ILogger<EventService> logger, IBsonFilter<Event> bsonFilter) : base(logger, ConnectionStringTypes.Eventos)
    {
        _repositoryBase = repositoryBase;
        _bsonFilter = bsonFilter;
    }

    public async Task<Event> CreateEvent(Event evento)
    {
        try
        {
            if (evento == null)
            {
                throw new NullReferenceException("You sent a null object");
            }

            var result = await _repositoryBase.CreateDocumentAsync<Event>(_collectionName, evento);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

    public async Task<List<Event>> GetAllEvents()
    {
        try
        {
            var result = await _repositoryBase.GetAllDocument<Event>(_collectionName);

            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }

    }

    public async Task<Event> GetEvents(Guid eventKey)
    {
        try
        {
            var filterGetEvent = _bsonFilter.FilterDefinition<Event>("EventKey", eventKey);

            var result = await _repositoryBase.GetDocument<Event>(_collectionName, filterGetEvent);

            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
    public async Task<bool> DeleteEvent(Guid eventKey)
    {
        try
        {
            var filterDeletEvent = _bsonFilter.FilterDefinition<Event>("EventKey", eventKey);

            var result = await _repositoryBase.DeleteDocument<Event>(_collectionName, filterDeletEvent);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

    public async Task<bool> UpdateEvent<T>(string filterDefinitionField, string filterDefinitionParam, string filterUpdateDefinitionField,
        string filterUpdateDefinitionParan)
    {
        try
        {
            var filterUpdate = _bsonFilter.FilterDefinitionUpdate(filterDefinitionField, filterDefinitionParam, filterUpdateDefinitionField, filterUpdateDefinitionParan, out UpdateDefinition<Event> update);

            var result = await _repositoryBase.UpdateDocument<Event>(_collectionName, filterUpdate, update);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
}