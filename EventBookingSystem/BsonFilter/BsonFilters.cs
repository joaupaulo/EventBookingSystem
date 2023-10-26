using MongoDB.Driver;

namespace EventBookingSystem.BsonFilter;

public class BsonFilter<T> : IBsonFilter<T>
{
    public FilterDefinition<T> FilterDefinition<T>(string filterDefinitionField, string filterDefinitionParam)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(filterDefinitionField, filterDefinitionParam);
        return filter;
    }

    public FilterDefinition<T> FilterDefinition<T>(string filterDefinitionField, Guid filterDefinitionParam)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(filterDefinitionField, filterDefinitionParam);
        return filter;
    }

    public FilterDefinition<T> FilterDefinitionUpdate(string filterDefinitionField, string filterDefinitionParam,
        string filterUpdateDefinitionField, string filterUpdateDefinitionParam, out UpdateDefinition<T> update)
    {
        var filter = Builders<T>.Filter.Eq(filterDefinitionField, filterDefinitionParam);
        update = Builders<T>.Update.Set(filterUpdateDefinitionField, filterUpdateDefinitionParam);
        return filter;
    }
}