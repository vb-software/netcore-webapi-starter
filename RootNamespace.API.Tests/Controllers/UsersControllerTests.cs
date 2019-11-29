using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Moq;
using RootNamespace.API.Controllers;
using RootNamespace.Entities.Domain.Mongo;
using RootNamespace.Entities.DTO;
using RootNamespace.Repositories.Interfaces.Domain.Mongo;
using Xunit;

namespace RootNamespace.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UsersController(_mockUserRepo.Object);
        }
        [Fact]
        public async Task GetUserByIdTest()
        {
            User mockUser = new User();
            var mockGuid = Guid.NewGuid();
            _mockUserRepo.Setup(repo => repo.GetUserById(mockGuid)).ReturnsAsync(mockUser);

            mockUser.Guid = mockGuid;

            var result = await _controller.GetUserById(mockGuid);

            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task GetAllUsersTest()
        {
            _mockUserRepo.Setup(repo => repo.GetUsers()).ReturnsAsync(new List<User> { new User(), new User(), new User() });

            var result = await _controller.GetAllUsers();

            Assert.IsType<List<User>>(result);
            Assert.Equal(3, result.Count());
        }
    }
}