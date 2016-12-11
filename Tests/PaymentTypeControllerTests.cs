using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Controllers;
using Accounts.Core.Repositories.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;
using Moq;
using NUnit.Framework;
using ListItem = Accounts.Core.Models.ListItem;
using PaymentTypes = Accounts.Core.Models.PaymentTypes;

namespace Tests
{

    [TestFixture]
    public class PaymentTypeControllerTests
    {
        private Mock<IPaymentTypeRepository> PaymentTypeRepositoryMock;
        private PaymentTypeController PaymentTypeController;
        private Guid guid;
        private Mock<IPaymentTypeModelBuilder> paymentTypeModelBuilderMock;

        private void Setup()
        {
            PaymentTypeRepositoryMock = new Mock<IPaymentTypeRepository>();
            paymentTypeModelBuilderMock=new Mock<IPaymentTypeModelBuilder>();
            guid = Guid.NewGuid();

            var items = new List<ListItem>
            {
                new ListItem
                {
                    Text = "Test",
                    Value = guid.ToString()
                }
            };

            PaymentTypeRepositoryMock.Setup(x => x.GetPaymentTypes()).Returns(Task.FromResult(items));
            paymentTypeModelBuilderMock.Setup(x => x.BuildViewModel(It.IsAny<PaymentTypes>()))
                .Returns(new Accounts.Models.PaymentTypes());

            PaymentTypeRepositoryMock.Setup(x => x.GetPaymentType(guid)).Returns(Task.FromResult(new PaymentTypes { Id = guid, Title = "Hemang" }));
            PaymentTypeRepositoryMock.Setup(x => x.AddPaymentType(It.IsAny<PaymentTypes>())).Returns(Task.FromResult(guid));
            PaymentTypeRepositoryMock.Setup(x => x.UpdatePaymentType(It.IsAny<PaymentTypes>())).Returns(Task.FromResult(1));

            PaymentTypeController = new PaymentTypeController(PaymentTypeRepositoryMock.Object, paymentTypeModelBuilderMock.Object);


        }

        [Test]
        public async Task ShouldHaveIndexViewForIndexPage()
        {
            Setup();

            var viewResult = await PaymentTypeController.Index() as ViewResult;

            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [Test]
        public async Task ShouldHavePaymentTypesForIndexPage()
        {
            Setup();

            var viewResult = await PaymentTypeController.Index() as ViewResult;

            Assert.IsInstanceOf(typeof(SelectListModel), viewResult.Model);
        }

        [Test]
        public async Task ShouldHaveSelectListModelWithValuesSetForIndexPage()
        {
            Setup();
            var viewResult = await PaymentTypeController.Index() as ViewResult;

            var controllerModel = viewResult.Model as SelectListModel;

            Assert.That(controllerModel.ListItems.Count(), Is.EqualTo(1));
            Assert.That(controllerModel.ListItems.First().Text, Is.EqualTo("Test"));
            Assert.That(controllerModel.ListItems.First().Value, Is.EqualTo(guid.ToString()));
        }


        [Test]
        public void ShouldHaveAddViewForAddPage()
        {
            Setup();

            var viewResult = PaymentTypeController.Add() as ViewResult;

            Assert.AreEqual("Add", viewResult.ViewName);
        }

        [Test]
        public void ShouldHavePaymentTypesForAddPage()
        {
            Setup();

            var viewResult = PaymentTypeController.Add() as ViewResult;

            Assert.IsInstanceOf(typeof(Accounts.Models.PaymentTypes), viewResult.Model);
        }

        [Test]
        public async Task ShouldNotCallPaymentTypeRepositoryIfModelIsInvalid()
        {
            Setup();
            PaymentTypeController.ModelState.AddModelError("test", "test");
            var actionResult = await PaymentTypeController.Add(new Accounts.Models.PaymentTypes()) as ViewResult;

            PaymentTypeRepositoryMock.Verify(x => x.AddPaymentType(It.IsAny<PaymentTypes>()), Times.Never);


            Assert.IsInstanceOf(typeof(Accounts.Models.PaymentTypes), actionResult.Model);
        }

        [Test]
        public async Task ShouldCallPaymentTypeRepositoryIfModelIsValid()
        {
            Setup();
            
            await PaymentTypeController.Add(new Accounts.Models.PaymentTypes());

            PaymentTypeRepositoryMock.Verify(x => x.AddPaymentType(It.IsAny<PaymentTypes>()), Times.Once);
        }

        [Test]
        public async Task ShouldRedirectToIndexPageIfPaymentTypeAddedSuccessfully()
        {
            Setup();

            RedirectToRouteResult result = await PaymentTypeController.Add(It.IsAny<Accounts.Models.PaymentTypes>()) as RedirectToRouteResult;

            Assert.AreEqual("Index",result.RouteValues["Action"]);
            Assert.AreEqual("PaymentType",result.RouteValues["Controller"]);
        }



        [Test]
        public async Task ShouldHaveUpdateViewAndPaymentTypesForUpdatePage()
        {
            Setup();

            var viewResult = await PaymentTypeController.Update(guid) as ViewResult;
            Assert.AreEqual("Update", viewResult.ViewName);
            Assert.IsInstanceOf(typeof(Accounts.Models.PaymentTypes), viewResult.Model);

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ShouldThrowExceptionIfPaymentTypeIsNotFoundOnTheUpdatePage()
        {
            Setup();
            PaymentTypeRepositoryMock.Setup(x => x.UpdatePaymentType(It.IsAny<PaymentTypes>())).Returns(Task.FromResult(0));
            await PaymentTypeController.Update(It.IsAny<Guid>());
        }



        [Test]
        public async Task ShouldRedirectToIndexPageIfPaymentTypeUpdatedSuccessfully()
        {
            Setup();

            RedirectToRouteResult result = await PaymentTypeController.Update(It.IsAny<Accounts.Models.PaymentTypes>()) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("PaymentType", result.RouteValues["Controller"]);
        }


        [Test]
        public async Task ShouldNotCallPaymentTypeRepositoryIfModelIsInvalidForUpdatePage()
        {
            Setup();
            PaymentTypeController.ModelState.AddModelError("test", "test");
            var actionResult = await PaymentTypeController.Update(new Accounts.Models.PaymentTypes()) as ViewResult;

            PaymentTypeRepositoryMock.Verify(x => x.UpdatePaymentType(It.IsAny<PaymentTypes>()), Times.Never);


            Assert.IsInstanceOf(typeof(Accounts.Models.PaymentTypes), actionResult.Model);
        }

        [Test]
        public async Task ShouldCallPaymentTypeRepositoryIfModelIsValidForUpdatePage()
        {
            Setup();

            await PaymentTypeController.Update(new Accounts.Models.PaymentTypes());

            PaymentTypeRepositoryMock.Verify(x => x.UpdatePaymentType(It.IsAny<PaymentTypes>()), Times.Once);
        }


    }
}