using EventBookingSystem.BsonFilter;
using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Repository;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventBookingSystem.Service;

public class EventService : RepositoryBase, IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private string _collectionName = "event-collection";
    private readonly IBsonFilter<Evento> _bsonFilter;

    public EventService(IRepositoryBase repositoryBase, ILogger<EventService> logger, IBsonFilter<Evento> bsonFilter) : base(logger, ConnectionStringType.Eventos )
    {
         _repositoryBase = repositoryBase;
         _bsonFilter = bsonFilter;
    }
    
    public async Task<Evento> CreateEvent(Evento evento)
    {
        try
        {
            if (evento == null)
            {
                throw new NullReferenceException("You sent a null object");
            }
            
            var result = await _repositoryBase.CreateDocumentAsync<Evento>(_collectionName, evento );
          
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        } 
    }
    
    public async Task<List<Evento>> GetAllEvents()
    {
        try
        {
            var result = await _repositoryBase.GetAllDocument<Evento>(_collectionName);

            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }

    }
    
    public async Task<Evento> GetEvents(string eventKey)
    {
        try
        {
            var filterGetEvent = _bsonFilter.FilterDefinition<Evento>("EventKey", eventKey); 
           
            var result = await _repositoryBase.GetDocument<Evento>(_collectionName,filterGetEvent);
            
            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
    public async Task<bool> DeleteEvent(string eventKey)
    {
        try
        {
            var filterDeletEvent = _bsonFilter.FilterDefinition<Evento>("EventKey", eventKey);
            
            var result = await _repositoryBase.DeleteDocument<Evento>(_collectionName, filterDeletEvent);

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
         var filterUpdate =  _bsonFilter.FilterDefinitionUpdate(filterDefinitionField, filterDefinitionParam, filterUpdateDefinitionField, filterUpdateDefinitionParan,  out UpdateDefinition<Evento> update);
            
         var result = await _repositoryBase.UpdateDocument<Evento>(_collectionName, filterUpdate, update);
            
         return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
}