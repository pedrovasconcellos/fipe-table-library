using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.UploadService.Repositories.Interfaces;

namespace Vasconcellos.FipeTable.UploadService.Repositories
{
    public class MongoDBRepository : IRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public MongoDBRepository(ILogger logger, string connectionString)
        {
            this._logger = logger;
            this._connectionString = connectionString;
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var mongoUrl = MongoUrl.Create(this._connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public bool HaveReferenceIdGreaterOrEquals(long referenceId)
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(FipeReference).Name;
                var collection = database
                    .GetCollection<FipeReference>(collectionName);

                return collection
                    .AsQueryable<FipeReference>()
                    .Any(x => x.Id >= referenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IList<T>> GetAllAsync<T>()
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
                _logger.LogError(ex, ex.Message);
                return new List<T>();
            }
        }

        public async Task<bool> InsertOneAsync<T>(T entity)
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
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertManyAsync<T>(IList<T> entities)
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
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
