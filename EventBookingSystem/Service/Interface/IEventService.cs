using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using MongoDB.Bson;

namespace EventBookingSystem.Service.Interface;

public interface IEventService
{
    Task<Evento> CreateEvent(EventoRequest evento);
    Task<Evento> GetEvents(string eventoId);
    Task<List<Evento>> GetAllEvents();
    Task<bool> DeleteEvent(string Id);
    Task<bool> UpdateEvent(Evento eventoRequest, string EventKey);
}