using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventBookingSystem.Model;

public class Reserva
{
   [BsonId]
   public ObjectId Id { get; set; }
   public Guid EventKey { get; set; }
   public int NumeroParticipante { get; set; }
   public DateTime DataReserva { get; set; }
   public decimal ValorTotal { get; private set; }
   public ReservaStatus Status { get; private set; }
   public List<Participante> Participantes { get;  set; }
   public  EventoReservado EventoReservado { get; set; }

   
   public Reserva()
   {
      ValorTotal = CalcularValorTotal();
      Status = ReservaStatus.Pendente;
   }

   public void AtualizarStatus(ReservaStatus novoStatus)
   {
      Status = novoStatus;
   }

   private decimal CalcularValorTotal()
   {
         decimal valorPorParticipante = 50.00M;

         if (NumeroParticipante < 0)
         {
            throw new InvalidOperationException("O número de participantes não pode ser negativo.");
         }

         decimal valorTotal = valorPorParticipante * NumeroParticipante;

         return valorTotal;
   }
   
}