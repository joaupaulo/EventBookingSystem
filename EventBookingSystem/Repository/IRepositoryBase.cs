using EventBookingSystem.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventBookingSystem.Repository;

public interface IRepositoryBase
{
      Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
      Task<List<T>> GetAllDocument<T>(string collectionName);
      Task<T> GetDocument<T>(string collectionName, Guid Key );
      Task<bool> DeleteDocument<T>(string collectionName, Guid Key);
      Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, T update);
}