using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventBookingSystem.Model;

public class EventoReservado
{
    
    [BsonId]
    public ObjectId _Id { get;  set; }
    public Guid EventKey { get;  set; }
    public string Nome { get;  set; }
    public DateTime Data { get;  set; }
    public string Local { get;  set; }
    public int CapacidadeMaxima { get;  set; }
    public decimal Preco { get;  set; }
    public string Descricao { get;  set; }
}