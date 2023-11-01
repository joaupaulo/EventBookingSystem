using EventBookingSystem.Model;
using EventBookingSystem.Service.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;

namespace EventBookingSystem.Service
{
    public class PDFGenerator : IPDFGenerator
    {
        private readonly ILogger<PDFGenerator> _logger;
        public PDFGenerator(ILogger<PDFGenerator> logger)
        {
            _logger = logger;
        }
        public byte[] GeneratePDF(Booking booking)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document doc = new Document();
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

                    doc.Open();

                    Paragraph titleReserva = new Paragraph("Booking details");
                    titleReserva.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleReserva);

                    doc.Add(new Paragraph("Booking Key: " + booking.BookingKey));
                    doc.Add(new Paragraph("Event Key: " + booking.EventKey));
                    doc.Add(new Paragraph("Participant Number: " + booking.ParticipantNumber));
                    doc.Add(new Paragraph("Date Booking: " + booking.DateBooking.ToShortDateString()));
                    Paragraph titleParticipantes = new Paragraph("Participants");
                    titleParticipantes.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleParticipantes);

                    foreach (var participante in booking.Participants)
                    {
                        doc.Add(new Paragraph("Participant Name: " + participante.Nome));
                        doc.Add(new Paragraph("Participant Email: " + participante.Email));
                        doc.Add(new Paragraph("Participant Number: " + participante.Telefone));
                        doc.Add(new Paragraph("Participant CPF: " + participante.CPF));
                        doc.Add(new Paragraph("Participant Registration Date: " + participante.DataInscricao.ToShortDateString()));
                        doc.NewPage();
                    }

                    Paragraph titleEvento = new Paragraph("Evento Details");
                    titleEvento.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleEvento);
                        
                    doc.Add(new Paragraph("Evento Key: " + booking.Event.EventKey));
                    doc.Add(new Paragraph("Event Name: " + booking.Event.Name));
                    doc.Add(new Paragraph("Event Date: " + booking.Event.Date.ToShortDateString()));
                    doc.Add(new Paragraph("Event place: " + booking.Event.Place));
                    doc.Add(new Paragraph("Maximum Capacity Event: " + booking .Event.MaximumCapacity));
                    doc.Add(new Paragraph("Event Price: " + booking .Event.Price));    
                    doc.Add(new Paragraph("Event Description: " + booking.Event.Description));

                    doc.Close();

                    return memoryStream.ToArray(); 
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while generate the PDF:" + ex.Message);
                }

                return null;
            }
        }

    }
}
