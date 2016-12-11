using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Controllers;
using Accounts.Core.Repositories.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;
using AutoMapper;
using Moq;
using NUnit.Framework;
using ListItem = Accounts.Core.Models.ListItem;
using UserModel = Accounts.Core.Models.UserModel;

namespace Tests
{

    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IMapper> _autoMapperMock;
        private UserController userController;
        private Guid guid;

        private void Setup()
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

        [Test]
        public async Task ShouldHaveIndexViewForIndexPage()
        {
            Setup();

            var viewResult = await userController.Index() as ViewResult;

            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [Test]
        public async Task ShouldHaveUserModelForIndexPage()
        {
            Setup();

            var viewResult = await userController.Index() as ViewResult;

            Assert.IsInstanceOf(typeof(SelectListModel), viewResult.Model);
        }

        [Test]
        public async Task ShouldHaveSelectListModelWithValuesSetForIndexPage()
        {
            Setup();
            var viewResult = await userController.Index() as ViewResult;

            var controllerModel = viewResult.Model as SelectListModel;

            Assert.That(controllerModel.ListItems.Count(), Is.EqualTo(1));
            Assert.That(controllerModel.ListItems.First().Text, Is.EqualTo("Test"));
            Assert.That(controllerModel.ListItems.First().Value, Is.EqualTo(guid.ToString()));
        }


        [Test]
        public void ShouldHaveAddViewForAddPage()
        {
            Setup();

            var viewResult = userController.Add() as ViewResult;

            Assert.AreEqual("Add", viewResult.ViewName);
        }

        [Test]
        public void ShouldHaveUserModelForAddPage()
        {
            Setup();

            var viewResult = userController.Add() as ViewResult;

            Assert.IsInstanceOf(typeof(Accounts.Models.UserModel), viewResult.Model);
        }

        [Test]
        public async Task ShouldNotCallUserRepositoryIfModelIsInvalid()
        {
            Setup();
            userController.ModelState.AddModelError("test", "test");
            var actionResult = await userController.Add(new Accounts.Models.UserModel()) as ViewResult;

            userRepositoryMock.Verify(x => x.AddUser(It.IsAny<UserModel>()), Times.Never);


            Assert.IsInstanceOf(typeof(Accounts.Models.UserModel), actionResult.Model);
        }

        [Test]
        public async Task ShouldCallUserRepositoryIfModelIsValid()
        {
            Setup();

            await userController.Add(new Accounts.Models.UserModel());

            userRepositoryMock.Verify(x => x.AddUser(It.IsAny<UserModel>()), Times.Once);
        }

        [Test]
        public async Task ShouldRedirectToIndexPageIfUserAddedSuccessfully()
        {
            Setup();

            RedirectToRouteResult result = await userController.Add(It.IsAny<Accounts.Models.UserModel>()) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("User", result.RouteValues["Controller"]);
        }



        [Test]
        public async Task ShouldHaveUpdateViewAndUserModelForUpdatePage()
        {
            Setup();
            _autoMapperMock.Setup(x => x.Map<Accounts.Models.UserModel>(It.IsAny<UserModel>())).Returns(new Accounts.Models.UserModel());
            var viewResult = await userController.Update(guid) as ViewResult;

            Assert.AreEqual("Update", viewResult.ViewName);
            Assert.IsInstanceOf(typeof(Accounts.Models.UserModel), viewResult.Model);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ShouldThrowExceptionIfUserIsNotFoundOnTheUpdatePage()
        {
            Setup();
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<UserModel>())).Returns(Task.FromResult(0));
            await userController.Update(It.IsAny<Guid>());
        }

        [Test]
        public async Task ShouldRedirectToIndexPageIfUserUpdatedSuccessfully()
        {
            Setup();

            RedirectToRouteResult result = await userController.Update(It.IsAny<Accounts.Models.UserModel>()) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("User", result.RouteValues["Controller"]);
        }


        [Test]
        public async Task ShouldNotCallUserRepositoryIfModelIsInvalidForUpdatePage()
        {
            Setup();
            userController.ModelState.AddModelError("test", "test");
            var actionResult = await userController.Update(new Accounts.Models.UserModel()) as ViewResult;

            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<UserModel>()), Times.Never);

            Assert.IsInstanceOf(typeof(Accounts.Models.UserModel), actionResult.Model);
        }

        [Test]
        public async Task ShouldCallUserRepositoryIfModelIsValidForUpdatePage()
        {
            Setup();

            await userController.Update(new Accounts.Models.UserModel());

            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<UserModel>()), Times.Once);
        }

    }
}