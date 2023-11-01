using EventBookingSystem.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventBookingSystem.Repository;

public class RepositoryBase : IRepositoryBase
{
    private readonly ILogger<RepositoryBase> _logger;
    protected readonly IMongoDatabase MongoDatabase;
    protected readonly MongoClientSettings MongoClientSettings;
    private string _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    public RepositoryBase(ILogger<RepositoryBase> logger, ConnectionStringTypes connectionStringType = ConnectionStringTypes.Eventos)
    {
        MongoClientSettings = MongoClientSettings.FromConnectionString(_connectionString);
        _logger = logger;
        switch (connectionStringType)
        {
            case ConnectionStringTypes.Eventos:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("UserRegister");
                break;
            case ConnectionStringTypes.Home:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("home");
                break;
            case ConnectionStringTypes.Reserva:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("reserva");
                break;
        }

    }

    public async Task<T> CreateDocumentAsync<T>(string collectionName, T Document)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            await collection.InsertOneAsync(Document);

            return Document;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw;
        }
    }


    public async Task<List<T>> GetAllDocument<T>(string collectionName)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var filter = new BsonDocument();

            var result = await collection.FindAsync<T>(filter);

            result.ToList().Count();

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw;
        }
    }

    public async Task<T> GetDocument<T>(string collectionName, FilterDefinition<T> filter)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw;
        }
    }

    public async Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            throw;
        }
    }

    public async Task<bool> DeleteDocument<T>(string collectionName, FilterDefinition<T> filter)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
            throw;
        }
    }
}


