using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Vasconcellos.FipeTable.ConsoleApp.Services
{
    public class MongoDBService
    {
        private static bool bsonWasConfigured = false;
        private readonly string _connectionString;

        public MongoDBService(string connectionString)
        {
            this._connectionString = connectionString;
            if (!bsonWasConfigured)
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                bsonWasConfigured = true;
            }
        }

        public async Task<bool> SaveAsync<T>(ILogger log, IList<T> entity)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                await collection.InsertManyAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var mongoUrl = MongoUrl.Create(this._connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}