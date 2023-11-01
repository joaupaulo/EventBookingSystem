using EventBookingSystem.Model;

namespace EventBookingSystem.Service.Interface
{
    public interface IPDFGenerator
    {
        byte[] GeneratePDF(Booking reserva);
    }
}
