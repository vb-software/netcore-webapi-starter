using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Entities.Domain;
using Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return new List<User>
            {
                new User {
                    FirstName = "Steve",
                    LastName = "VandenBrink",
                    ID = 1,
                    DateOfBirth = new System.DateTime(1982, 9, 14)
                }
            };
        }

        [Route("{id:long}")]
        [HttpGet]
        public async Task<User> GetUserById(long id)
        {
            return new User
            {
                FirstName = "Mock",
                LastName = "User",
                DateOfBirth = DateTime.Now,
                ID = id
            };
        }

        [HttpPost]
        public async Task<ApiResponse> NewUser([FromBody] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(userDTO);
                    user.ID = 999;
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