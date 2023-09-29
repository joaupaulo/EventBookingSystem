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

    public EventService(IRepositoryBase repositoryBase, ILogger<EventService> logger ) : base(logger, ConnectionStringType.Eventos )
    {
         _repositoryBase = repositoryBase;
    }
    
    public async Task<Evento> CreateEvent(EventoRequest evento)
    {
        try
        {
            
            if (evento == null)
            {
                throw new NullReferenceException("");
            }

            var eventoHandler = EventMapper(evento);

            var result = await _repositoryBase.CreateDocumentAsync<Evento>(_collectionName, eventoHandler);
            
            return result;
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

    private static Evento EventMapper(EventoRequest evento)
    {
        Evento eventoRequest = Evento.CriarNovoEvento
        (
            evento.Nome,
            evento.Data,
            evento.Local,
            evento.CapacidadeMaxima,
            evento.Preco,
            evento.Descricao
        );

        foreach (var participanteDto in evento.Participantes)
        {
            Participante participante =
                Participante.CriarNovoParticipante(participanteDto.Nome, participanteDto.Email, participanteDto.Phone);
            eventoRequest.AdicionarParticipante(participante);
        }

        return eventoRequest;
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
        finally
        {
            _logger.LogInformation($"FindAll done in collection{_collectionName}");
        }
       
    }
    
    public async Task<Evento> GetEvents(string id)
    {
        try
        {
            var result = await _repositoryBase.GetDocument<Evento>(_collectionName,id);

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
    public async Task<bool> DeleteEvent(string Id)
    {
        try
        {
            var result = await _repositoryBase.DeleteDocument<Evento>(_collectionName, Id );

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
    
    public async Task<bool> UpdateEvent(EventoRequest eventoRequest, string Id)
    {
        try
        {
            var eventoDocument = eventoRequest.ToBsonDocument();
            
            var filter = Builders<Evento>.Filter.Eq("_id", new ObjectId(Id));
            
            var result = await _repositoryBase.UpdateDocument<Evento>(_collectionName,filter, eventoDocument);

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