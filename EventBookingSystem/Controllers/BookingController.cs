using EventBookingSystem.Model;
using EventBookingSystem.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Configurations;

[ApiController]
[Route("/booking")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateEvent(Booking reserva)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReserva = await _bookingService.CreateReserva(reserva);

            _logger.LogInformation($"Booking created sucessfully. ID: {createdReserva}");

            return Ok(createdReserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while create booking.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("bookingKey/{key}")]
    public async Task<IActionResult> GetReservaByKey(string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest();
            }

            if (!Guid.TryParse(key , out Guid bookingKey))
            {
                _logger.LogError("You did not send a valid key");
                return BadRequest();
            }

            var reserva = await _bookingService.GetReserva(bookingKey);

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting booking per key.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("getAllBooking")]
    public async Task<IActionResult> GetAllReserva()
    {
        try
        {
            var reserva = await _bookingService.GetAllReservas();

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all booking.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{bookingKey}")]
    public async Task<IActionResult> DeleteReserva(string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest();
            }

            if (!Guid.TryParse(key, out Guid reservaKey))
            {
                _logger.LogError("bookingKey is dont guid valid.");
                return BadRequest();
            }

            var deleteReserva = await _bookingService.DeleteReserva(reservaKey);

            if (!deleteReserva)
            {
                return NotFound();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while delete booking.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateEvent(string requestFilterDefField, string requestFilterDefParam, string FilterUpdatField, string FilterUpdateParam)
    {
        try
        {
            bool result = await _bookingService.UpdateReserva(requestFilterDefField, requestFilterDefParam,
                FilterUpdatField, FilterUpdateParam
            );

            if (result)
            {
                return Ok("Update booking sucess");
            }

            return NotFound("Booking not found or cant be update.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the booking : " + ex.Message);
        }
    }

}