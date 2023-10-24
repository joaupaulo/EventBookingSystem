using System.ComponentModel.DataAnnotations;
using EventBookingSystem.Model.Validations;
using MongoDB.Bson;

namespace EventBookingSystem.Model;

public class Evento
{
    public ObjectId EventoId { get;  set; }
    [Required(ErrorMessage = "Send a event key")]
    public Guid EventKey { get;  set; }
    [Required(ErrorMessage = "Fill name of event")]
    public string Nome { get;  set; }
    [FutureDate(ErrorMessage = "The event should be in future")]
    public DateTime Data { get;  set; }
    [Required(ErrorMessage = "The location of the event is mandatory")]
    public string Local { get;  set; }
    [Range(1, Int32.MaxValue, ErrorMessage = "The maximum capacity must be greater than zero")]
    public int CapacidadeMaxima { get; set; }
    [Range(0, Double.MaxValue, ErrorMessage = "The maximum capacity must be greater than zero")]
    public decimal Preco { get;  set; }
    public string Descricao { get;  set; }

   
}
