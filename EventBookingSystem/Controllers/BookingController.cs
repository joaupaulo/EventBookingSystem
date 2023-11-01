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
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBooking = await _bookingService.CreateReserva(booking);

            _logger.LogInformation($"Booking created sucessfully. ID: {createdBooking.Id}");

            return Ok(createdBooking);
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

            var booking = await _bookingService.GetReserva(bookingKey);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting booking per key.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("getAllBooking")]
    public async Task<IActionResult> GetAllBooking()
    {
        try
        {
            var booking = await _bookingService.GetAllReservas();

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all booking.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{bookingKey}")]
    public async Task<IActionResult> DeleteBooking(string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest();
            }

            if (!Guid.TryParse(key, out Guid bookingKey))
            {
                _logger.LogError("bookingKey is dont guid valid.");
                return BadRequest();
            }

            var deleteBooking = await _bookingService.DeleteReserva(bookingKey);

            if (!deleteBooking)
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
    public async Task<IActionResult> UpdateBooking(string requestFilterDefField, string requestFilterDefParam, string FilterUpdatField, string FilterUpdateParam)
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