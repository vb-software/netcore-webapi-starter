using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
#if (!useMongoDB)
using RootNamespace.Entities.Domain;
#else
using RootNamespace.Entities.Domain.Mongo;
using MongoDB.Bson;
using RootNamespace.Repositories.Interfaces.Domain.Mongo;
#endif
using RootNamespace.Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RootNamespace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        #if (useMongoDB)
        private readonly IUserRepository _userRepo;
        #endif

        #if (!useMongoDB)
        public UsersController(IMapper mapper, ILogger<UsersController> logger)
        #else
        public UsersController(IMapper mapper, ILogger<UsersController> logger, IUserRepository userRepo)
        #endif
        {
            _mapper = mapper;
            _logger = logger;
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
                    FirstName = "Steve",
                    LastName = "VandenBrink",
                    ID = 1,
                    DateOfBirth = new System.DateTime(1982, 9, 14)
                }
            };
            #else
            return await _userRepo.GetUsers();
            #endif
        }

        [Route("{id:long}")]
        [HttpGet]
        #if (!useMongoDB)
        public async Task<User> GetUserById(long id)
        #else
        public async Task<User> GetUserById(ObjectId id)
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

        [HttpPost]
        public async Task<ApiResponse> NewUser([FromBody] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #if (!useMongoDB)
                    var user = _mapper.Map<User>(userDTO);
                    user.ID = 999;
                    #else
                    var user = await _userRepo.AddOrReplaceUser(_mapper.Map<User>(userDTO));
                    #endif
                    
                    return new ApiResponse("Created successfully", user, 201);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error while trying to create user.");
                    throw;
                }
            }
            else
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }
    }
}