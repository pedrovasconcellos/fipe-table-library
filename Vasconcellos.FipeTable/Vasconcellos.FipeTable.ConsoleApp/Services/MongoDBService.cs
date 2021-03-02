using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Linq;
using Vasconcellos.FipeTable.Types.Entities;

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

        private IMongoDatabase GetMongoDatabase()
        {
            var mongoUrl = MongoUrl.Create(this._connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public bool HaveReferenceCodeGreaterOrEquals(ILogger log, long referenceCode)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(FipeVehicleInformation).Name;
                var collection = database
                    .GetCollection<FipeVehicleInformation>(collectionName);

                return collection
                    .AsQueryable<FipeVehicleInformation>()
                    .Any(x => x.FipeReferenceCode >= referenceCode);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertOneAsync<T>(ILogger log, T entity)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                await collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertManyAsync<T>(ILogger log, IList<T> entities)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                await collection.InsertManyAsync(entities);
                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IList<T>> GetAllAsync<T>(ILogger log)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                return await collection.AsQueryable().ToListAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new List<T>();
            }
        }

        public async Task<bool> SaveVehicleBrands(ILogger logger, IList<FipeVehicleBrand> brands)
        {
            var brandsSelected = await this.GetAllAsync<FipeVehicleBrand>(logger);
            var brandsInserted = brands
                .Where(x => !brandsSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (brandsInserted.Count == 0)
                return false;

            return await this.InsertManyAsync(logger, brandsInserted);
        }

        public async Task<bool> SaveVehicleModels(ILogger logger, IList<FipeVehicleModel> models)
        {
            var modelsSelected = await this.GetAllAsync<FipeVehicleModel>(logger);
            var modelsInserted = models
                .Where(x => !modelsSelected.Any(y => x.Id == y.Id))
                .ToList();

            if (modelsInserted.Count == 0)
                return false;

            return await this.InsertManyAsync(logger, modelsInserted);
        }

        public async Task<bool> SaveVehicles(ILogger logger, IList<FipeVehicleInformation> vehicles)
        {
            return await this.InsertManyAsync(logger, vehicles);
        }
    }
}