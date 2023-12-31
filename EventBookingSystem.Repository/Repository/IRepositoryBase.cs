﻿using MongoDB.Driver;

namespace EventBookingSystem.Repository;

public interface IRepositoryBase
{
    Task<T> CreateDocumentAsync<T>(string collectionName, T Document);
    Task<List<T>> GetAllDocument<T>(string collectionName);
    Task<T> GetDocument<T>(string collectionName, FilterDefinition<T> filter);
    Task<bool> UpdateDocument<T>(string collectionName, FilterDefinition<T> filter, UpdateDefinition<T> update);
    Task<bool> DeleteDocument<T>(string collectionName, FilterDefinition<T> filter);
}