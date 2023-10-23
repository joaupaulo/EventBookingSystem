using System.Diagnostics.Eventing.Reader;
using EventBookingSystem.Model.DTOs;
using MongoDB.Bson;

namespace EventBookingSystem.Model;

public class Reserva
{
   public ObjectId Id { get; private set; }
   public Guid EventKey { get; private set; }
  
   public EventoDto EventoId { get; private set; }
   public int NumeroParticipante { get; private set; }
   public DateTime DataReserva { get; private set; }
   public List<Participante> Participantes { get; private set; }

   private Reserva(Guid eventKey, EventoDto eventoId, int numeroParticipante, DateTime dataReserva, List<Participante> NewParticipantes)
   {
      if(eventKey == Guid.Empty)
         throw new ArgumentException("You send me guid empty");
      if(numeroParticipante < 0 )
         throw new ArgumentException("number participant less than zero");
      if(DataReserva < DateTime.Now)
         throw new ArgumentException("Date should be more than today");
      if(!Participantes.Any())
         AdicionarParticipante(NewParticipantes);
     
      EventKey = Guid.NewGuid();
      NumeroParticipante = numeroParticipante;
      DataReserva = dataReserva;
      Participantes = NewParticipantes;
      
   }

   public void AdicionarParticipante(List<Participante> participantes)
   {
      foreach (var participante in participantes)
      {
         if (participante == null)
            throw new ArgumentNullException(nameof(participante), "O participante não pode ser nulo.");

         if (Participantes.Count == 0)
            throw new InvalidOperationException("Número de participantes deve ser maior que 0.");

         if (Participantes.Any(p => p.Email == participante.Email))
            throw new InvalidOperationException("Este participante já está inscrito neste evento.");

         Participantes.Add(participante);
      }
   }
   public static Reserva CreateReserva(Guid eventKey, EventoDto eventoId, int numeroParticipante,
      DateTime dataReserva, List<Participante> NewParticipantes)
   {
      return new Reserva(eventKey, eventoId, numeroParticipante, dataReserva, NewParticipantes);
   }
   
}