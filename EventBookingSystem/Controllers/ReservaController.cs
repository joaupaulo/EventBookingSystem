using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Controllers;

[ApiController]
[Route("/reserva")]
public class ReservaController : ControllerBase
{
    private readonly IReservaService _reservaService;
    private readonly ILogger<ReservaController> _logger;

    public ReservaController(IReservaService reservaService, ILogger<ReservaController> logger)
    {
        _reservaService = reservaService;
        _logger = logger;
    }

    [HttpGet("Get/{reservaKey}")]
    public async Task<IActionResult> GetReserva(string reservaKey)
    {
        try
        {
            if (string.IsNullOrEmpty(reservaKey))
            {
                return BadRequest("O ID da reserva não foi fornecido.");
            }

            var reserva = await _reservaService.GetReserva(reservaKey);

            if (reserva == null)
            {
                return NotFound($"Reserva com ID {reservaKey} não encontrada.");
            }

            return Ok(reserva);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao buscar a reserva.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllReservas()
    {
        try
        {
            var reservas = await _reservaService.GetAllReservas();

            if (reservas == null || !reservas.Any())
            {
                return NotFound("Nenhuma reserva encontrada.");
            }

            return Ok(reservas);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao buscar todas as reservas.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    [HttpDelete("Delete/{reservaKey}")]
    public async Task<IActionResult> DeleteReserva(string reservaKey)
    {
        try
        {
            if (string.IsNullOrEmpty(reservaKey))
            {
                return BadRequest("O ID da reserva não foi fornecido.");
            }

            var deletedReserva = await _reservaService.DeleteReserva(reservaKey);

            if (deletedReserva == null)
            {
                return NotFound($"Reserva com ID {reservaKey} não encontrada.");
            }

            return Ok($"Reserva com ID {reservaKey} foi excluída com sucesso.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao excluir a reserva.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateReserva(EventoReservado reserva)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReserva = await _reservaService.CreateReserva(reserva);

            _logger.LogInformation($"Reserva criada com sucesso. ID: {createdReserva.EventKey}");

            return Ok(new ReservaResponse()
            {
              Resultado = "Sua reserva foi feita com sucesso!",
              Reserva = createdReserva
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao criar a reserva.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }

    [HttpPut("Update/{reservaKey}")]
    public async Task<IActionResult> UpdateReserva(string reservaKey, [FromBody] EventoReservado reserva)
    {
        try
        {
            if (string.IsNullOrEmpty(reservaKey))
            {
                return BadRequest("O ID da reserva não foi fornecido.");
            }

            if (reserva == null)
            {
                return BadRequest("Os dados da reserva para atualização não foram fornecidos.");
            }

            var updatedReserva = await _reservaService.UpdateReserva(reserva, reservaKey);

            if (updatedReserva == null)
            {
                return NotFound($"Reserva com ID {reservaKey} não encontrada.");
            }

            return Ok(updatedReserva);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao atualizar a reserva.");
            return StatusCode(500, "Ocorreu um erro interno.");
        }
    }
}
