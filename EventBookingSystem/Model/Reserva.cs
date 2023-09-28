using MongoDB.Bson;

namespace EventBookingSystem.Model;

public class Reserva
{
   public ObjectId Id { get; set; }
   public Evento EventoId { get; set; }
   public Usuario UsuarioId { get; set; }
   public int NumeroParticipante { get; set; }
   public DateTime DataReserva { get; set; }
}