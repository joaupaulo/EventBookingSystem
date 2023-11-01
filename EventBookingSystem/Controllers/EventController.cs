using EventBookingSystem.Model;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers;

[ApiController]
[Route("/event")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpGet("eventkey/{eventKey}")]
    public async Task<IActionResult> GetEventByKey(string eventKey)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(eventKey))
            {
                return BadRequest();
            }

            if (!Guid.TryParse(eventKey, out Guid key))
            {
                _logger.LogError("eventKey is not a valid guid.");
                return BadRequest();
            }

            var evento = await _eventService.GetEvents(key);

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

            var evento = await _eventService.GetAllEvents();

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

            if (!Guid.TryParse(Key, out Guid eventKey))
            {
                _logger.LogError("eventKey is not a valid guid.");
                return BadRequest();
            }

            var deleteEvent = await _eventService.DeleteEvent(eventKey);

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

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateEvent(string requestFilterDefField, string requestFilterDefParam, string FilterUpdatField, string FilterUpdateParam)
    {
        try
        {
            bool result = await _eventService.UpdateEvent<Event>(requestFilterDefField, requestFilterDefParam,
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
            return StatusCode(500, "An error occurred while updating the event: " + ex.Message);
        }
    }


    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(Event eventoDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEvent = await _eventService.CreateEvent(eventoDto);

            _logger.LogInformation($"Event created sucessfully. ID: {createdEvent.EventKey}");

            return Ok(createdEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create event.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}