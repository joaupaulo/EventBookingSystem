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
    
    [HttpGet("Get/{eventKey}")]
    public async Task<IActionResult> GetEvent(string eventKey)
    {
        try
        {
            if (string.IsNullOrEmpty(eventKey))
            {
                return BadRequest("O ID do evento não foi fornecido.");
            }

            var evento = await _eventService.GetEvents(eventKey);

            if (evento == null)
            {
                return NotFound($"Evento com ID {eventKey} não encontrado.");
            }

            return Ok(evento);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao buscar o evento.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllEvents()
    {
        try
        {
            var eventos = await _eventService.GetAllEvents();

            if (eventos == null || !eventos.Any())
            {
                return NotFound("Nenhum evento encontrado.");
            }

            return Ok(eventos);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao buscar todos os eventos.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }
    [HttpDelete("Delete/{eventoId}")]
    public async Task<IActionResult> DeleteEvent(string eventoId)
    {
        try
        {
            if (string.IsNullOrEmpty(eventoId))
            {
                return BadRequest("O ID do evento não foi fornecido.");
            }

            var deletedEvent = await _eventService.DeleteEvent(eventoId);

            if (deletedEvent == null)
            {
                return NotFound($"Evento com ID {eventoId} não encontrado.");
            }

            return Ok($"Evento com ID {eventoId} foi excluído com sucesso.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao excluir o evento.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }
    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(EventoRequest evento)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEvent = await _eventService.CreateEvent(evento);

            _logger.LogInformation($"Evento criado com sucesso. ID: {createdEvent._Id}");


            return Ok(new EventoResponse()
            {
                Result = "Evento Criado, Parabéns!",
                evento = createdEvent
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPut("Update/{eventoId}")]
    public async Task<IActionResult> UpdateEvent(string eventKey, [FromBody] Evento eventoRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(eventKey))
            {
                return BadRequest("O ID do evento não foi fornecido.");
            }

            if (eventoRequest == null)
            {
                return BadRequest("Os dados do evento para atualização não foram fornecidos.");
            }
            
            var updatedEvent = await _eventService.UpdateEvent(eventoRequest,eventKey);

            if (updatedEvent == null)
            {
                return NotFound($"Evento com ID {eventKey} não encontrado.");
            }

            return Ok(updatedEvent);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao atualizar o evento.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

}