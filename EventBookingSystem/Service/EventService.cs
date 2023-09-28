using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Repository;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MongoDB.Bson;

namespace EventBookingSystem.Service;

public class EventService : RepositoryBase, IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private string _collectionName = "event-collection";

    public EventService(IRepositoryBase repositoryBase, ILogger<EventService> logger ) : base(logger, ConnectionStringType.Eventos )
    {
         _repositoryBase = repositoryBase;
    }
    
    public async Task<EventoDto> CreateEvent<EventoDto>(EventoDto evento)
    {
        try
        {
            if (evento == null)
            {
                throw new NullReferenceException("");
            }
            
            var result = await _repositoryBase.CreateDocumentAsync<EventoDto>(_collectionName, evento );
          
          return evento;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        } finally
        {
            _logger.LogInformation($"Crete done in collection{_collectionName}");
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
        finally
        {
            _logger.LogInformation($"FindAll done in collection{_collectionName}");
        }
       
    }
    
    public async Task<EventoDto> GetEvents<EventoDto>(Evento eventoDto)
    {
        try
        {
            var result = await _repositoryBase.GetDocument<EventoDto>(_collectionName,eventoDto);

            return result;

        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"FindAll done in collection{_collectionName}");
        }
    }
    public async Task<bool> DeleteEvent<T>(Evento evento)
    {
        try
        {
            var result = await _repositoryBase.DeleteDocument<T>(_collectionName, evento);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Delete done in collection{_collectionName}");
        }
    }
    
    public async Task<bool> UpdateEvent<T>(BsonDocument filterUpdate, BsonDocument filter)
    {
        try
        {
            var result = await _repositoryBase.UpdateDocument<T>(_collectionName, filterUpdate, filter);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Update done in collection{_collectionName}");
        }
    }
}