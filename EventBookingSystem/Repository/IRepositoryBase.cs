using EventBookingSystem.Model;
using MongoDB.Bson;

namespace EventBookingSystem.Repository;

public interface IRepositoryBase
{
      Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
      Task<List<T>> GetAllDocument<T>(string collectionName);
      Task<T> GetDocument<T>(string collectionName, Evento evento);
      Task<bool> UpdateDocument<T>(string collectionName, BsonDocument filterUpdate, BsonDocument filter);
      Task<bool> DeleteDocument<T>(string collectionName, Evento evento);
      



}