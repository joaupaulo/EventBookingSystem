using EventBookingSystem.Model.Validations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model;

public class Event
{
    [BsonId]
    public ObjectId _Id { get; set; }
    [Required(ErrorMessage = "Send a event key")]
    public Guid EventKey { get; set; }
    [Required(ErrorMessage = "Fill name of event")]
    public string Name { get; set; }
    [FutureDate(ErrorMessage = "The event should be in future")]
    public DateTime Date { get; set; }
    [Required(ErrorMessage = "The location of the event is mandatory")]
    public string Place { get; set; }
    [Range(1, Int32.MaxValue, ErrorMessage = "The maximum capacity must be greater than zero")]
    public int MaximumCapacity { get; set; }
    [Range(0, Double.MaxValue, ErrorMessage = "The maximum capacity must be greater than zero")]
    public decimal Price { get; set; }
    public string Description { get; set; }
}
