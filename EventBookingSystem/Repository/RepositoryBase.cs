using System.Xml.XPath;
using EventBookingSystem.Model;
using EventBookingSystem.Model.DTOs;
using EventBookingSystem.Model.MongoDbConfig;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace EventBookingSystem.Repository;

public class RepositoryBase : IRepositoryBase
{
    private readonly ILogger<RepositoryBase> _logger;
    protected readonly IMongoDatabase MongoDatabase;
    protected readonly MongoClientSettings MongoClientSettings;
    private string ConnectionString => "mongodb+srv://joaopaulo123:1799jp@cluster0.8rok3.mongodb.net/?authSource=admin";

    public RepositoryBase(ILogger<RepositoryBase> logger,ConnectionStringType connectionStringType = ConnectionStringType.Eventos)
    {
        MongoClientSettings = MongoClientSettings.FromConnectionString(ConnectionString);
        _logger = logger;
        switch (connectionStringType)
        {
            case ConnectionStringType.Eventos:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("UserRegister");
                break;
            case ConnectionStringType.Home:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("home");
                break;
            case ConnectionStringType.Reserva:
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("UserRegister");
                break;
        }

    }
    
    public async Task<T> CreateDocumentAsync<T>(string collectionName, T document)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);
            
             await collection.InsertOneAsync(document);

             return document;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}, have find {count}");
        }
    }
    

    public async Task<List<T>> GetAllDocument<T>(string collectionName)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var filter = new BsonDocument();

            var result = await collection.FindAsync(filter);

            var documents = await result.ToListAsync();

            count = documents.Count;

            return documents;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}, have find {count}");
        }
    }

    public async Task<T> GetDocument<T>(string collectionName, Guid Key)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventKey", Key);

            var result = await collection.Find(filter).FirstOrDefaultAsync();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }

    }

    public async Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, T update) 
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.ReplaceOneAsync(filter,update);

            return result.ModifiedCount > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
        finally
        {
            _logger.LogInformation($"Find done in collection{collectionName}");
        }
    }
   
    public async Task<bool> DeleteDocument<T>(string collectionName, Guid Key)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventKey", Key);

            var result = await collection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _logger.LogInformation($"Delete done in collection {collectionName}");
        }
    }
}


