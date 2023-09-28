using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Service.Interface;
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
    
    [HttpGet("key/{key}")]
    public ActionResult<Evento> GetEventByKey()
    {
        return Ok();
    }

    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(EventoDto evento)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEvent = await _eventService.CreateEvent<EventoDto>(evento);

            _logger.LogInformation($"Evento criado com sucesso. ID: {createdEvent.EventoId}");


            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       

    }

}