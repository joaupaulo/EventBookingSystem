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

    public ReservaService(IRepositoryBase repositoryBase, ILogger<ReservaService> logger) : base(logger, ConnectionStringType.Reserva)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
    }

    public async Task<Reserva> CreateReserva(Reserva reserva)
    {
        try
        {
            if (reserva == null)
            {
                throw new NullReferenceException("Os dados da reserva não foram fornecidos.");
            }
            
            var result = await _repositoryBase.CreateDocumentAsync<Reserva>(_collectionName, reserva);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Create done in collection {_collectionName}");
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
        finally
        {
            _logger.LogInformation($"FindAll done in collection {_collectionName}");
        }
    }

      public async Task<Reserva> GetReserva(string reservaKey)
      {
          try
          {
              if (!Guid.TryParse(reservaKey, out Guid key))
              {
                  _logger.LogError("eventKey não é um Guid válido.");
                  return null; 
              }
              
              var result = await _repositoryBase.GetDocument<Reserva>(_collectionName, key);

              return result;
          }
          catch (Exception ex)
          {
              _logger.LogError($"{ex}");
              throw;
          }
          finally
          {
              _logger.LogInformation($"Find done in collection {_collectionName}");
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
            
            var result = await _repositoryBase.DeleteDocument<Reserva>(_collectionName, key);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Delete done in collection {_collectionName}");
        }
    }

    public async Task<bool> UpdateReserva(Reserva reserva, string reservaKey)
    {
        try
        {
            var filter = Builders<Reserva>.Filter.Eq("ReservaKey", reservaKey);

            var result = await _repositoryBase.UpdateDocument<Reserva>(_collectionName, filter, reserva);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Update done in collection {_collectionName}");
        }
    }
}