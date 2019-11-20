using RootNamespace.Entities.Settings;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Threading;

namespace RootNamespace.Repositories.Mongo
{
    public abstract class MongoRepositoryBase
    {
        protected MongoDbSettings _mongoDbSettings;

        static MongoRepositoryBase() { }

        protected MongoClient ConnectToServer()
        {
            if (_mongoDbSettings != null &&
                !string.IsNullOrWhiteSpace(_mongoDbSettings.ConnectionString))
            {
                return ConnectToServer(_mongoDbSettings.ConnectionString);
            }

            return ConnectToServer(null);
        }

        protected MongoClient ConnectToServer(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                // Default to using camel case for properties
                var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("camelCase", conventionPack, t => true);

                var mongoServer = new MongoServerAddress(_mongoDbSettings.Server);

                var mongoSettings = new MongoClientSettings
                {
                    ApplicationName = _mongoDbSettings.ApplicationName,
                    ConnectTimeout = TimeSpan.FromSeconds(_mongoDbSettings.Timeout),
                    Server = mongoServer
                };

                if (_mongoDbSettings.Admin != null &&
                    !string.IsNullOrWhiteSpace(_mongoDbSettings.Admin.Trim()))
                {
                    var mongoCredential =
                        MongoCredential.CreateCredential(
                            "admin",
                            _mongoDbSettings.Admin,
                            _mongoDbSettings.Password);
                    mongoSettings.Credential = mongoCredential;
                }

                return new MongoClient(mongoSettings);
            }
            return new MongoClient(connectionString);
        }

        private IMongoDatabase GetMongoDatabase()
        {
            return GetMongoDatabase(null);
        }

        private IMongoDatabase GetMongoDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                return ConnectToServer().GetDatabase(_mongoDbSettings.ApplicationName);
            }

            return ConnectToServer().GetDatabase(databaseName);
        }

        private IMongoDatabase _db;

        protected virtual IMongoDatabase Db
        {
            get { return LazyInitializer.EnsureInitialized(ref _db, GetMongoDatabase); }
        }
    }
}
