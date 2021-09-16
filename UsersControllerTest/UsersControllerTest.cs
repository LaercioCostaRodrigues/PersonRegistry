using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonRegistry.Controllers;
using PersonRegistry.Interfaces;
using PersonRegistry.Models;
using System.Linq;
using Xunit;

namespace UsersControllerTest
{
    public class UsersControllerTest
    {
        [Fact]
        public void GetUsers_ReturnAllUsers()
        {
            // Arrange
            int count = 3;
            var fakeUsers = A.CollectionOfFake<User>(count).AsEnumerable();
            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.GetAll()).Returns(fakeUsers.ToList());

            // Act
            var actionResult = UserController.GetUsers();

            // Assert
            Assert.Equal(count, actionResult.Value.ToList().Count);
        }

        [Fact]
        public void GetUser_NullUser_ReturnNotFound()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUser = A.Fake<User>();
            fakeUser.Id = id;

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find("NonexistentId")).Returns(null);

            // Act
            var actionResult = UserController.GetUser("NonexistentId");

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void GetUser_UserById_ReturnUser()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUser = A.Fake<User>();
            fakeUser.Id = id;
            fakeUser.FirstName = "Dennis";
            fakeUser.Surname = "Ritchie";
            fakeUser.Age = 70;

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find(id)).Returns(fakeUser);

            // Act
            var actionResult = UserController.GetUser(id);

            // Assert
            Assert.IsType<ActionResult<User>>(actionResult);
            Assert.Equal(id, actionResult.Value.Id);
            Assert.Equal("Dennis", actionResult.Value.FirstName);
            Assert.Equal("Ritchie", actionResult.Value.Surname);
            Assert.Equal(70, actionResult.Value.Age);
        }

        [Fact]
        public void PutUser_NullUser_ReturnNotFound()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUserBind = A.Fake<UserBind>();
            var fakeUser = A.Fake<User>();
            fakeUser.Id = id;

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find(id)).Returns(null);

            // Act
            var iactionResult = UserController.PutUser(id, fakeUserBind);

            // Assert
            Assert.IsType<NotFoundResult>(iactionResult);
        }

        [Fact]
        public void PutUser_WithWrongId_ReturnBadRequest()
        {
            // Arrange
            string id = "NonexistentId";
            var fakeUserBind = A.Fake<UserBind>();
            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);

            // Act
            var iactionResult = UserController.PutUser(id, fakeUserBind);

            // Assert
            Assert.IsType<BadRequestResult>(iactionResult);
        }

        [Fact]
        public void PutUser_DbUpdateConcurrencyException_ReturnNotFound()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUserBind = A.Fake<UserBind>();
            var fakeUser = A.Fake<User>();
            fakeUser.Id = id;

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find(id)).Returns(fakeUser);
            A.CallTo(() => fakeRepository.SaveChanges()).Throws(new DbUpdateConcurrencyException());
            A.CallTo(() => fakeRepository.UserExists(id)).Returns(false);

            // Act
            var iactionResult = UserController.PutUser(id, fakeUserBind);

            // Assert
            Assert.IsType<NotFoundResult>(iactionResult);
        }

        [Fact]
        public void PutUser_WithExistentId_ReturnNoContent()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUserBind = A.Fake<UserBind>();
            var fakeUser = A.Fake<User>();
            fakeUser.Id = id;

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find(id)).Returns(fakeUser);

            // Act
            var iactionResult = UserController.PutUser(id, fakeUserBind);

            // Assert
            Assert.IsType<NoContentResult>(iactionResult);
        }

        [Fact]
        public void PostUser_DbUpdateException_ReturnConflit()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUserBind = A.Fake<UserBind>();

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.IdGenerator()).Returns(id);
            A.CallTo(() => fakeRepository.SaveChanges()).Throws(new DbUpdateException());
            A.CallTo(() => fakeRepository.UserExists(id)).Returns(true);

            // Act
            var actionResult = UserController.PostUser(fakeUserBind);

            // Assert
            Assert.IsType<ConflictResult>(actionResult.Result);
        }

        [Fact]
        public void PostUser_RightRequestBody_UserActionResult()
        {
            // Arrange
            var fakeUserBind = A.Fake<UserBind>();

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);

            // Act
            var actionResult = UserController.PostUser(fakeUserBind);

            // Assert
            Assert.IsType<ActionResult<User>>(actionResult);
        }

        [Fact]
        public void DeleteUser_WithNoexistentUser_ReturnNotFound()
        {
            // Arrange
            string id = "NonexistentId";
            var fakeUser = A.Fake<User>();

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);
            A.CallTo(() => fakeRepository.Find(id)).Returns(null);

            // Act
            var iactionResult = UserController.DeleteUser(id);

            // Assert
            Assert.IsType<NotFoundResult>(iactionResult);
        }

        [Fact]
        public void DeleteUser_WithExistentUser_NoContent()
        {
            // Arrange
            string id = "b4f5a-b4f5a-b4f5a-b4f5a-b4f5a";
            var fakeUser = A.Fake<User>();

            var fakeRepository = A.Fake<IUserRepository>();
            var UserController = new UsersController(fakeRepository);

            // Act
            var iactionResult = UserController.DeleteUser(id);

            // Assert
            Assert.IsType<NoContentResult>(iactionResult);
        }

    }

}
