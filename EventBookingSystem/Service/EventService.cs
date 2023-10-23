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
    
    public async Task<Evento> CreateEvent(EventoDto evento)
    {
        try
        {
            if (evento == null)
            {
                throw new NullReferenceException("");
            }
            
            var eventos =  Model.Evento.CriarNovoEvento( evento.Nome, evento.Data, evento.Local, 
                evento.CapacidadeMaxima, evento.Preco, evento.Descricao);
            
            var result = await _repositoryBase.CreateDocumentAsync<Evento>(_collectionName, eventos );
          
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        } 
    }
    
    public async Task<List<EventoDto>> GetAllEvents<EventoDto>()
    {
        try
        {
            var result = await _repositoryBase.GetAllDocument<EventoDto>(_collectionName);

            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }

    }
    
    public async Task<EventoDto> GetEvents<EventoDto>(string eventKey)
    {
        try
        {
            var filterGetEvent = _bsonFilter.FilterDefinition<EventoDto>("EventKey", eventKey); 
           
            var result = await _repositoryBase.GetDocument<EventoDto>(_collectionName,filterGetEvent);
            
            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
    public async Task<bool> DeleteEvent<T>(string eventKey)
    {
        try
        {
            var filterDeletEvent = _bsonFilter.FilterDefinition<EventoDto>("EventKey", eventKey);
            
            var result = await _repositoryBase.DeleteDocument<EventoDto>(_collectionName, filterDeletEvent);

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