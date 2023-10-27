using EventBookingSystem.Model.Validations;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model;

public class Reserva
{
    public ObjectId Id { get; set; }
    [Required(ErrorMessage = "You sent reserva key key null")]
    public Guid ReservaKey { get; set; }
    [Required(ErrorMessage = "You sent event key null")]
    public Guid EventKey { get; set; }
    [Range(1, Int32.MaxValue, ErrorMessage = "Number of participants must be greater than zero.")]
    public int NumeroParticipante { get; set; }
    [FutureDate(ErrorMessage = "The event should be in future")]
    public DateTime DataReserva { get; set; }
    [UniqueParticipants(ErrorMessage = "Participants must be unique by email.")]
    [UniqueCPF(ErrorMessage = "CPF must be unique.")]
    public List<Participante> Participantes { get; set; }
    public Evento Evento { get; set; }

}