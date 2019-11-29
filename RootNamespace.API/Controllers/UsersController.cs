using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if (!useMongoDB)
using RootNamespace.Entities.Domain;
#else
using RootNamespace.Entities.Domain.Mongo;
using RootNamespace.Repositories.Interfaces.Domain.Mongo;
#endif
using Microsoft.AspNetCore.Mvc;
#if (useJwt)
using Microsoft.AspNetCore.Authorization;
#endif

namespace RootNamespace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    #if (useJwt)
    [Authorize]
    #endif
    public class UsersController : ControllerBase
    {
        #if (useMongoDB)
        private readonly IUserRepository _userRepo;
        #endif

        #if (!useMongoDB)
        public UsersController()
        #else
        public UsersController(IUserRepository userRepo)
        #endif
        {
            #if (useMongoDB)
            _userRepo = userRepo;
            #endif
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            #if (!useMongoDB)
            return new List<User>
            {
                new User {
                    FirstName = "Mock",
                    LastName = "User",
                    ID = 1,
                    DateOfBirth = DateTime.Now
                }
            };
            #else
            return await _userRepo.GetUsers();
            #endif
        }

        [Route("{id}")]
        [HttpGet]
        #if (!useMongoDB)
        public async Task<User> GetUserById(long id)
        #else
        public async Task<User> GetUserById(Guid id)
        #endif
        {
            #if (!useMongoDB)
            return new User
            {
                FirstName = "Mock",
                LastName = "User",
                DateOfBirth = DateTime.Now,
                ID = id
            };
            #else
            return await _userRepo.GetUserById(id);
            #endif
        }
    }
}