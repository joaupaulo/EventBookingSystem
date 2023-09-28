﻿using System.Xml.XPath;
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
                MongoDatabase = new MongoClient(MongoClientSettings).GetDatabase("reserva");
                break;
        }

    }
    
    public async Task<T> CreateDocumentAsync<T>(string collectionName, T Document)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);
            
             await collection.InsertOneAsync(Document);

            return Document;
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

            var result = await collection.FindAsync<T>(filter);

            count = result.ToList().Count();

            return result.ToList();
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

    public async Task<T> GetDocument<T>(string collectionName, Evento evento)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventoId", evento.EventoId);

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

    public async Task<bool> UpdateDocument<T>(string collectionName,  BsonDocument filterUpdate, BsonDocument filter)
    {
        var count = 0;
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            var result = await collection.UpdateOneAsync(filter, filterUpdate);

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
   
    public async Task<bool> DeleteDocument<T>(string collectionName, Evento evento)
    {
        try
        {
            var collection = MongoDatabase.GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter.Eq("EventoId", evento.EventoId);

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


