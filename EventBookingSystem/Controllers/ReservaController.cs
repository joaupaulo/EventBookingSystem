using EventBookingSystem.Model;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Configurations;

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

    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(Reserva reserva)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var createdReserva= await _reservaService.CreateReserva(reserva);
           
            _logger.LogInformation($"Reserva created sucessfully. ID: {createdReserva}");
            
            return Ok(createdReserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create reserva.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpGet("reservaKey/{key}")]
    public async Task<IActionResult> GetReservaByKey(string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest();
            }

            var reserva = await _reservaService.GetReserva(key);

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting reserva per key.");
            return StatusCode(500, "An error occurred while processing your request.");
        } 
    }

    [HttpGet("getAllReservas")]
    public async Task<IActionResult> GetAllReserva()
    {
        try
        {
            var reserva = await _reservaService.GetAllReservas();

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all reservas.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{reservaKey}")]
    public async Task<IActionResult> DeleteReserva(string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest();
            }

            var deleteReserva = await _reservaService.DeleteReserva(key);

            if (!deleteReserva)
            {
                return NotFound();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete reserva.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateEvent(string requestFilterDefField, string requestFilterDefParam, string FilterUpdatField, string FilterUpdateParam)
    {
        try
        {
            bool result = await _reservaService.UpdateReserva(requestFilterDefField, requestFilterDefParam, 
                FilterUpdatField, FilterUpdateParam
            );

            if (result)
            {
                return Ok("Update reserva sucess");
            }

            return NotFound("Reserva not found or cant be update.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the reserva : " + ex.Message);
        }
    }
    
    }