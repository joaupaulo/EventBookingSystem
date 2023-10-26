using EventBookingSystem.BsonFilter;
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
    private readonly IPDFGenerator _pdfGenerator;
    private readonly IEmailSender _emailSender;

    public ReservaService(IRepositoryBase repositoryBase, ILogger<ReservaService> logger, IBsonFilter<Reserva> bsonFilter, IEventService eventService, IPDFGenerator pdfGenerator, IEmailSender emailSender) : base(logger, ConnectionStringType.Reserva)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
        _bsonFilter = bsonFilter;
        _eventService = eventService;
        _pdfGenerator = pdfGenerator;
        _emailSender = emailSender;
    }

    public async Task<Reserva> CreateReserva(Reserva reserva)
    {
        try
        {
            if (reserva == null)
            {
                throw new NullReferenceException("Booking details were not provided.");
            }

            var getEventforMapping = await _eventService.GetEvents(reserva.EventKey);

            if(getEventforMapping == null)
            {
                throw new ArgumentNullException("You be try create a reserva of event that dont exist");
            }

            reserva.Evento = getEventforMapping;

            var result = await _repositoryBase.CreateDocumentAsync<Reserva>(_collectionName, reserva);

           var newPdf = _pdfGenerator.GeneratePDF(result);

            var emailParticipantes = result.Participantes.Select( x => x.Email).ToList();

            string subject = "Sua reserva foi realizada!";
            string body = "Olá tudo bem ? sua reserva para o evento X foi realizada";
            string attachmentFileName = "reserva.pdf";
            byte[] attachmentData = newPdf;

            foreach (var email in emailParticipantes)
            {
                string toAddress = email;
             
                bool sentEmail = _emailSender.SendEmailWithAttachment(toAddress, subject, body, attachmentData, attachmentFileName);
            }

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