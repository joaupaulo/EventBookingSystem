using EventBookingSystem.Model;
using EventBookingSystem.Service.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml.Linq;

namespace EventBookingSystem.Service
{
    public class PDFGenerator : IPDFGenerator
    {
        private readonly ILogger<PDFGenerator> _logger;
        public  PDFGenerator(ILogger<PDFGenerator> logger) 
        {
            _logger = logger;
        }
        public byte[] GeneratePDF(Reserva reserva)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document doc = new Document();
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

                    doc.Open();

                    Paragraph titleReserva = new Paragraph("Reserva Details");
                    titleReserva.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleReserva);

                    doc.Add(new Paragraph("Reserva Key: " + reserva.ReservaKey));
                    doc.Add(new Paragraph("Event Key: " + reserva.EventKey));
                    doc.Add(new Paragraph("Numero de Participantes: " + reserva.NumeroParticipante));
                    doc.Add(new Paragraph("Data da Reserva: " + reserva.DataReserva.ToShortDateString()));

                    Paragraph titleParticipantes = new Paragraph("Participantes");
                    titleParticipantes.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleParticipantes);

                    foreach (var participante in reserva.Participantes)
                    {
                        doc.Add(new Paragraph("Nome do Participante: " + participante.Nome));
                        doc.Add(new Paragraph("Email do Participante: " + participante.Email));
                        doc.Add(new Paragraph("Telefone do Participante: " + participante.Telefone));
                        doc.Add(new Paragraph("CPF do Participante: " + participante.CPF));
                        doc.Add(new Paragraph("Data de Inscrição do Participante: " + participante.DataInscricao.ToShortDateString()));
                        doc.NewPage();
                    }

                    Paragraph titleEvento = new Paragraph("Evento Details");
                    titleEvento.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleEvento);

                    doc.Add(new Paragraph("Evento Key: " + reserva.Evento.EventKey));
                    doc.Add(new Paragraph("Nome do Evento: " + reserva.Evento.Nome));
                    doc.Add(new Paragraph("Data do Evento: " + reserva.Evento.Data.ToShortDateString()));
                    doc.Add(new Paragraph("Local do Evento: " + reserva.Evento.Local));
                    doc.Add(new Paragraph("Capacidade Máxima do Evento: " + reserva.Evento.CapacidadeMaxima));
                    doc.Add(new Paragraph("Preço do Evento: " + reserva.Evento.Preco));
                    doc.Add(new Paragraph("Descrição do Evento: " + reserva.Evento.Descricao));

                    doc.Close();

                    return memoryStream.ToArray(); // Retorna o PDF em memória como um array de bytes
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while generate the PDF:" + ex.Message);
                }

                return null; // Em caso de erro, retorne null ou lide com o erro adequadamente
            }
        }

    }
}
