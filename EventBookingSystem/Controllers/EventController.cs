using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers;

[ApiController]
[Route("/event")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService,ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }
    
    [HttpGet("eventkey/{key}")]
    public async Task<IActionResult> GetEventByKey(string Key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                return BadRequest();
            }
           
            var evento = await _eventService.GetEvents<EventoDto>(Key);

            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting event per key.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    [HttpGet("getAllEvents")]
    public async Task<IActionResult> GetAllEvents()
    {
        try
        {

            var evento = await _eventService.GetAllEvents<EventoDto>();

            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all events.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpDelete("{eventKey}")]
    public async Task<IActionResult> DeleteEvent(string Key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                return BadRequest();
            }

            var deleteEvent = await _eventService.DeleteEvent<EventoDto>(Key);

            if (!deleteEvent)
            {
                return NotFound();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete event.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEvent(string requestFilterDefField, string requestFilterDefParam, string FilterUpdatField, string FilterUpdateParam)
    {
        try
        {
            bool result = await _eventService.UpdateEvent<Evento>(requestFilterDefField, requestFilterDefParam, 
                FilterUpdatField, FilterUpdateParam
            );

            if (result)
            {
                return Ok("Update Event Sucess");
            }

            return NotFound("Event not found or cant be update.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro ao atualizar o evento: " + ex.Message);
        }
    }
    
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(EventoDto eventoDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
           var createdEvent = await _eventService.CreateEvent(eventoDto);
           
           _logger.LogInformation($"Evento criado com sucesso. ID: {createdEvent.EventoId}");
            
            return Ok(createdEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create event.");
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }
    
}