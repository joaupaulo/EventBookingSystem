using EventBookingSystem.Model;
using iTextSharp.text;

namespace EventBookingSystem.Service.Interface
{
    public interface IPDFGenerator
    {
        byte[] GeneratePDF(Reserva reserva);
    }
}
