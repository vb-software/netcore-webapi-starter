using System.Collections.Generic;
using System.Threading.Tasks;
using RootNamespace.Entities.Domain.Mongo;
using RootNamespace.Entities.Settings;
using RootNamespace.Repositories.Domain.Mongo.Extensions;
using RootNamespace.Repositories.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using RootNamespace.Repositories.Interfaces.Domain.Mongo;

namespace RootNamespace.Repositories.Domain.Mongo
{
    public class UserRepository : MongoRepositoryBase, IUserRepository
    {
        public UserRepository(MongoDbSettings mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings;
        }

        public async Task<User> AddNewUser(User userToAdd)
        {
            await GetCollection().InsertOneAsync(userToAdd);
            return userToAdd;
        }

        public async Task<User> GetUserById(ObjectId userId)
        {
            var filter = Builders<User>.Filter.Eq("_id", userId);
            var userFromRepo = await GetCollection().Find(user => user.MongoId == userId).FirstOrDefaultAsync();
            return userFromRepo;
        }

        public async Task<List<User>> GetUsers()
        {
            return await GetCollection().Find(x => true).ToListAsync();
        }

        public async Task RemoveUser(User userToRemove)
        {
            await GetCollection().DeleteOneAsync(user => user.MongoId == userToRemove.MongoId);
        }

        private IMongoCollection<User> GetCollection()
        {
            var collection = Db.GetGlobalCollection<User>();
            return collection;
        }
    }
}