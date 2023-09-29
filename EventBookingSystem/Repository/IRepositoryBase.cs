using EventBookingSystem.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventBookingSystem.Repository;

public interface IRepositoryBase
{
      Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
      Task<List<T>> GetAllDocument<T>(string collectionName);
      Task<T> GetDocument<T>(string collectionName, string eventoId );
      Task<bool> DeleteDocument<T>(string collectionName, string Id);
      Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);
}