﻿using EventBookingSystem.BsonFilter;
using EventBookingSystem.Model;
using EventBookingSystem.Repository;
using EventBookingSystem.Service.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventBookingSystem.Service;

public class ReservaService : RepositoryBase, IReservaService
{
    private readonly ILogger<ReservaService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private string _collectionName = "reserva-register";
    private readonly IBsonFilter<Reserva> _bsonFilter;
    private readonly IEventService _eventService;

    public ReservaService(IRepositoryBase repositoryBase, ILogger<ReservaService> logger, IBsonFilter<Reserva> bsonFilter, IEventService eventService) : base(logger, ConnectionStringType.Reserva)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
        _bsonFilter = bsonFilter;
        _eventService = eventService;
    }

    public async Task<Reserva> CreateReserva(Reserva reserva)
    {
        try
        {
            if (reserva == null)
            {
                throw new NullReferenceException("Booking details were not provided.");
            }

            string eventKey = reserva.EventKey.ToString();

            var getEventforMapping = await _eventService.GetEvents(eventKey);

            reserva.Evento = getEventforMapping;

            var result = await _repositoryBase.CreateDocumentAsync<Reserva>(_collectionName, reserva);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
    
      public async Task<List<Reserva>> GetAllReservas()
    {
        try
        {
            var result = await _repositoryBase.GetAllDocument<Reserva>(_collectionName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

      public async Task<Reserva> GetReserva(string reservaKey)
      {
          try
          {
              if (!Guid.TryParse(reservaKey, out Guid key))
              {
                  _logger.LogError("You did not send a valid key");
                  return null; 
              }

              var filterGetReserva = _bsonFilter.FilterDefinition<Reserva>("ReservaKey", reservaKey);
              
              var result = await _repositoryBase.GetDocument<Reserva>(_collectionName, filterGetReserva);

              return result;
          }
          catch (Exception ex)
          {
              _logger.LogError($"{ex}");
              throw;
          }
      }

    public async Task<bool> DeleteReserva(string reservaKey)
    {
        try
        {
            if (!Guid.TryParse(reservaKey, out Guid key))
            {
                _logger.LogError("eventKey não é um Guid válido.");
                return false;
            }
           
            var filterDeletReserva = _bsonFilter.FilterDefinition<Reserva>("ReservaKey", reservaKey);
            
            var result = await _repositoryBase.DeleteDocument<Reserva>(_collectionName, filterDeletReserva);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

    public async Task<bool> UpdateReserva(string filterDefinitionField, string filterDefinitionParam, string filterUpdateDefinitionField, 
        string filterUpdateDefinitionParan)
    {
        try
        {
            var filterUpdate =  _bsonFilter.FilterDefinitionUpdate(filterDefinitionField, filterDefinitionParam, filterUpdateDefinitionField, filterUpdateDefinitionParan,  out UpdateDefinition<Reserva> update);

            var result = await _repositoryBase.UpdateDocument<Reserva>(_collectionName, filterUpdate, update);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
}