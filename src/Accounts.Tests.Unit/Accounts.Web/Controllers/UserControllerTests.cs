using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Web.Controllers;
using Accounts.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ListItem = Accounts.Core.Models.ListItem;
using UserModel = Accounts.Core.Models.UserModel;

namespace Accounts.Tests.Unit.Accounts.Web.Controllers
{


    public class UserControllerTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IMapper> _autoMapperMock;
        private UserController userController;
        private Guid guid;

        public UserControllerTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            _autoMapperMock = new Mock<IMapper>();
            guid = Guid.NewGuid();

            var items = new List<ListItem>
            {
                new ListItem
                {
                    Text = "Test",
                    Value = guid.ToString()
                }
            };

            userRepositoryMock.Setup(x => x.GetUsers()).Returns(Task.FromResult(items));

            userRepositoryMock.Setup(x => x.GetUser(guid)).Returns(Task.FromResult(new UserModel { Amount = 4, Id = guid, Name = "Hemang" }));
            userRepositoryMock.Setup(x => x.AddUser(It.IsAny<UserModel>())).Returns(Task.FromResult(guid));
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<UserModel>())).Returns(Task.FromResult(1));

            userController = new UserController(userRepositoryMock.Object, _autoMapperMock.Object);
        }

        [Fact]
        public async Task ShouldHaveIndexViewForIndexPage()
        {
            var viewResult = await userController.Index() as ViewResult;

            Assert.Equal("Index", viewResult.ViewName);
        }

        [Fact]
        public async Task ShouldHaveUserModelForIndexPage()
        {
            var viewResult = await userController.Index() as ViewResult;

            Assert.IsType<SelectListModel>(viewResult.Model);
        }

        [Fact]
        public async Task ShouldHaveSelectListModelWithValuesSetForIndexPage()
        {
            var viewResult = await userController.Index() as ViewResult;

            var controllerModel = viewResult.Model as SelectListModel;

            Assert.Single(controllerModel.ListItems);
            Assert.Equal("Test", controllerModel.ListItems.First().Text);
            Assert.Equal(controllerModel.ListItems.First().Value, guid.ToString());
        }

        [Fact]
        public void ShouldHaveAddViewForAddPage()
        {
            var viewResult = userController.Add() as ViewResult;

            Assert.Equal("Add", viewResult.ViewName);
        }

        [Fact]
        public void ShouldHaveUserModelForAddPage()
        {
            var viewResult = userController.Add() as ViewResult;

            Assert.IsType<global::Accounts.Web.Models.UserModel>(viewResult.Model);
        }

        [Fact]
        public async Task ShouldNotCallUserRepositoryIfModelIsInvalid()
        {
            userController.ModelState.AddModelError("test", "test");
            var actionResult = await userController.Add(new global::Accounts.Web.Models.UserModel()) as ViewResult;

            userRepositoryMock.Verify(x => x.AddUser(It.IsAny<UserModel>()), Times.Never);

            Assert.IsType<global::Accounts.Web.Models.UserModel>(actionResult.Model);
        }

        [Fact]
        public async Task ShouldCallUserRepositoryIfModelIsValid()
        {
            await userController.Add(new global::Accounts.Web.Models.UserModel());

            userRepositoryMock.Verify(x => x.AddUser(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task ShouldRedirectToIndexPageIfUserAddedSuccessfully()
        {
            RedirectToActionResult result = await userController.Add(It.IsAny<global::Accounts.Web.Models.UserModel>()) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }

        [Fact]
        public async Task ShouldHaveUpdateViewAndUserModelForUpdatePage()
        {
            _autoMapperMock.Setup(x => x.Map<global::Accounts.Web.Models.UserModel>(It.IsAny<UserModel>())).Returns(new global::Accounts.Web.Models.UserModel());
            var viewResult = await userController.Update(guid) as ViewResult;

            Assert.Equal("Update", viewResult.ViewName);
            Assert.IsType<global::Accounts.Web.Models.UserModel>(viewResult.Model);

        }

        [Fact]
        public async Task ShouldThrowExceptionIfUserIsNotFoundOnTheUpdatePage()
        {
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<UserModel>())).Returns(Task.FromResult(0));
            await Assert.ThrowsAsync<NullReferenceException>(() => userController.Update(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task ShouldRedirectToIndexPageIfUserUpdatedSuccessfully()
        {
            RedirectToActionResult result = await userController.Update(It.IsAny<global::Accounts.Web.Models.UserModel>()) as RedirectToActionResult;
            
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }

        [Fact]
        public async Task ShouldNotCallUserRepositoryIfModelIsInvalidForUpdatePage()
        {
            userController.ModelState.AddModelError("test", "test");
            var actionResult = await userController.Update(new global::Accounts.Web.Models.UserModel()) as ViewResult;

            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<UserModel>()), Times.Never);

            Assert.IsType<global::Accounts.Web.Models.UserModel>(actionResult.Model);
        }

        [Fact]
        public async Task ShouldCallUserRepositoryIfModelIsValidForUpdatePage()
        {
            await userController.Update(new global::Accounts.Web.Models.UserModel());

            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<UserModel>()), Times.Once);
        }
    }
}