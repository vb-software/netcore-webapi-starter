using System;
using System.Net;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RootNamespace.API.Controllers;
using RootNamespace.Entities.Domain.Mongo;
using RootNamespace.Entities.DTO;
using RootNamespace.Entities.Settings;
using RootNamespace.Repositories.Interfaces.Domain.Mongo;
using Xunit;

namespace RootNamespace.API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _jwtSettings = new JwtSettings { Secret = Guid.NewGuid().ToString() };
            _controller = new AuthController(_mockUserRepository.Object, _jwtSettings);
        }

        [Fact]
        public async Task RegisterInvalidModelTest()
        {
            _controller.ModelState.AddModelError("error", "error");

            try
            {
                var result = await _controller.Register(new UserForRegisterDto());
            }
            catch (Exception e)
            {
                Assert.IsType<ApiException>(e);
            }
        }

        [Fact]
        public async Task RegisterUserAlreadyExistsTest()
        {
            var userForRegisterDto = new UserForRegisterDto
            {
                Username = "fred",
                FirstName = "Fred",
                LastName = "Flinstone",
                Password = "loveswilma"
            };

            _mockUserRepository.Setup(repo => repo.UserExists(userForRegisterDto.Username)).ReturnsAsync(true);

            var result = await _controller.Register(userForRegisterDto);

            Assert.IsType<ApiResponse>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.True(result.IsError);
            Assert.Equal("User already exists with that username", result.ResponseException);
        }

        [Fact]
        public async Task RegisterUserValidTest()
        {
            var userForRegisterDto = new UserForRegisterDto
            {
                Username = "fred",
                FirstName = "Fred",
                LastName = "Flinstone",
                Password = "loveswilma"
            };

            var createdUser = new User
            {
                Username = "fred",
                FirstName = "Fred",
                LastName = "Flinstone"
            };

            _mockUserRepository.Setup(repo => repo.UserExists(userForRegisterDto.Username)).ReturnsAsync(false);
            _mockUserRepository.Setup(repo => repo.Register(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(createdUser);

            var result = await _controller.Register(userForRegisterDto);

            Assert.IsType<ApiResponse>(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(createdUser, result.Result);
        }

        [Fact]
        public async Task LoginUnauthorizedTest()
        {
            var result = await _controller.Login(new UserForLoginDto { Username = "", Password = "" });

            Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal((int)HttpStatusCode.Unauthorized, (result as UnauthorizedResult).StatusCode);
        }

        [Fact]
        public async Task LoginReturnsTokenTest()
        {
            _mockUserRepository.Setup(repo => repo.Login("", ""))
                .ReturnsAsync(new User
                {
                    FirstName = "Fred",
                    LastName = "Flinstone",
                    Username = "fred",
                    Guid = Guid.NewGuid()
                });

            var result = await _controller.Login(new UserForLoginDto { Username = "", Password = "" });

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, (result as OkObjectResult).StatusCode);
        }
    }
}