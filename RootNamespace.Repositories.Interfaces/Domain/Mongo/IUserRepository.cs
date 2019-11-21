using System.Collections.Generic;
using System.Threading.Tasks;
using RootNamespace.Entities.Domain.Mongo;
using RootNamespace.Repositories.Interfaces;
using MongoDB.Bson;

namespace RootNamespace.Repositories.Interfaces.Domain.Mongo
{
    public interface IUserRepository : IRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserById(ObjectId userId);
        Task<User> AddNewUser(User userToAdd);
        Task RemoveUser(User userToRemove);
        #if (useJwt)
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        #endif
    }
}