using MongoDB.Bson;

namespace EventBookingSystem.Model.DTOs;

public class EventoRequest
    {
        public ObjectId EventoId { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public int CapacidadeMaxima { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public int QuantiadeParticipates { get; set; }
        public List<ParticipanteRequest> Participantes { get; set; }
    }
