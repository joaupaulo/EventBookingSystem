using EventBookingSystem.BsonFilter;
using EventBookingSystem.Model;
using EventBookingSystem.Repository;
using EventBookingSystem.Service.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EventBookingSystem.Service;

public class BookingService : RepositoryBase, IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IRepositoryBase _repositoryBase;
    private string _collectionName = "reserva-register";
    private readonly IBsonFilter<Booking> _bsonFilter;
    private readonly IEventService _eventService;
    private readonly IPDFGenerator _pdfGenerator;
    private readonly IEmailSender _emailSender;

    public BookingService(IRepositoryBase repositoryBase, ILogger<BookingService> logger, IBsonFilter<Booking> bsonFilter, IEventService eventService, IPDFGenerator pdfGenerator, IEmailSender emailSender) : base(logger, ConnectionStringTypes.Reserva)
    {
        _repositoryBase = repositoryBase;
        _logger = logger;
        _bsonFilter = bsonFilter;
        _eventService = eventService;
        _pdfGenerator = pdfGenerator;
        _emailSender = emailSender;
    }

    public async Task<Booking> CreateReserva(Booking booking)
    {
        try
        {
            if (booking == null)
            {
                throw new NullReferenceException("Booking details were not provided.");
            }

            var getEventforMapping = await _eventService.GetEvents(booking.EventKey);

            if (getEventforMapping == null)
            {
                throw new ArgumentNullException("You be try create a reserva of event that dont exist");
            }

            booking.Event = getEventforMapping;

            var result = await _repositoryBase.CreateDocumentAsync<Booking>(_collectionName, booking);

            var newPdf = _pdfGenerator.GeneratePDF(result);

            var emailParticipantes = result.Participants.Select(x => x.Email).ToList();

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

    public async Task<List<Booking>> GetAllReservas()
    {
        try
        {
            var result = await _repositoryBase.GetAllDocument<Booking>(_collectionName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

    public async Task<Booking> GetReserva(Guid reservaKey)
    {
        try
        {
            var filterGetReserva = _bsonFilter.FilterDefinition<Booking>("ReservaKey", reservaKey);

            var result = await _repositoryBase.GetDocument<Booking>(_collectionName, filterGetReserva);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }

    public async Task<bool> DeleteReserva(Guid reservaKey)
    {
        try
        {
            var filterDeletReserva = _bsonFilter.FilterDefinition<Booking>("ReservaKey", reservaKey);

            var result = await _repositoryBase.DeleteDocument<Booking>(_collectionName, filterDeletReserva);

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
            var filterUpdate = _bsonFilter.FilterDefinitionUpdate(filterDefinitionField, filterDefinitionParam, filterUpdateDefinitionField, filterUpdateDefinitionParan, out UpdateDefinition<Booking> update);

            var result = await _repositoryBase.UpdateDocument<Booking>(_collectionName, filterUpdate, update);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
}