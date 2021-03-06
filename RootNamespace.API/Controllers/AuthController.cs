using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
#if (!useMongoDB)
using RootNamespace.Entities.Domain;
#endif
#if (useMongoDB)
using RootNamespace.Entities.Domain.Mongo;
#endif
using RootNamespace.Entities.DTO;
using RootNamespace.Entities.Settings;
#if (useMongoDB)
using RootNamespace.Repositories.Interfaces.Domain.Mongo;
#endif
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
#if (useMongoDB)
using AutoWrapper.Wrappers;
using AutoWrapper.Extensions;
using System.Net;
#endif

namespace RootNamespace.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        #if (useMongoDB)
        private readonly IUserRepository _userRepo;
        #endif
        private readonly JwtSettings _settings;

        public AuthController(
            #if (useMongoDB)
            IUserRepository userRepo, 
            #endif
            JwtSettings settings)
        {
            #if (useMongoDB)
            _userRepo = userRepo;
            #endif
            _settings = settings;
        }

        [HttpPost("register")]
		#if (useMongoDB)
        public async Task<ApiResponse> Register([FromBody] UserForRegisterDto userForRegisterDto)
		#else
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
		#endif
        {
            #if (useMongoDB)
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(userForRegisterDto.Username))
                {
                    userForRegisterDto.Username = userForRegisterDto.Username.Trim().ToLower();
                }

                if (await _userRepo.UserExists(userForRegisterDto.Username))
                {
                    return new ApiResponse((int)HttpStatusCode.BadRequest, "User already exists with that username");
                }

                var userToCreate = new User
                {
                    Username = userForRegisterDto.Username,
                    FirstName = userForRegisterDto.FirstName.Trim(),
                    LastName = userForRegisterDto.LastName.Trim()
                };

                var createUser = await _userRepo.Register(userToCreate, userForRegisterDto.Password.Trim());

                return new ApiResponse("Created successfully", createUser, 201);
            }
            else
            {
                throw new ApiException(ModelState.AllErrors());
            }
            #else
            return StatusCode(201);
			#endif
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            #if (useMongoDB)
            var userFromRepo = await _userRepo.Login(userForLoginDto.Username.Trim().ToLower(), userForLoginDto.Password.Trim());

            if (userFromRepo == null)
            {
                return Unauthorized();
            }
            #endif

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    #if (useMongoDB)
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Guid.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username),
                    new Claim(ClaimTypes.GivenName, $"{userFromRepo.FirstName} {userFromRepo.LastName}" )
                    #endif
                }),
                Expires = DateTime.Now.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { tokenString });
        }
    }
}