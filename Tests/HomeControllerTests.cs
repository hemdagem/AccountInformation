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
using PaymentModel = Accounts.Core.Models.PaymentModel;
using PaymentViewModel = Accounts.Models.PaymentViewModel;
using UserModel = Accounts.Core.Models.UserModel;

namespace Tests
{

    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IPaymentRepository> _paymentRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPaymentModelFactory> _paymentModelMock;
        private Mock<IMapper> _autoMapperMock;
        private HomeController _homeController;
        private Guid _guid;

        private void Setup()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _paymentModelMock = new Mock<IPaymentModelFactory>();
            _autoMapperMock = new Mock<IMapper>();
            _guid = Guid.NewGuid();

            var items = new List<ListItem>
            {
                new ListItem
                {
                    Text = "Test",
                    Value = _guid.ToString()
                }
            };

            var paymentModels = new List<PaymentModel> { new PaymentModel { Amount = 4, Id = _guid, PaidYearly = true, Paid = true, Date = DateTime.Today, Title = "Title" } };
            
            _userRepositoryMock.Setup(x => x.GetUsers()).Returns(Task.FromResult(items));
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<Guid>())).Returns(Task.FromResult(new UserModel()));

            _paymentRepositoryMock.Setup(x => x.GetPaymentsById(_guid)).Returns(Task.FromResult(paymentModels));
            _paymentRepositoryMock.Setup(x => x.AddPayment(It.IsAny<PaymentModel>())).Returns(Task.FromResult(_guid));
            _paymentRepositoryMock.Setup(x => x.UpdatePayment(It.IsAny<PaymentModel>())).Returns(Task.FromResult(1));
            _paymentRepositoryMock.Setup(x => x.DeletePayment(It.IsAny<Guid>())).Returns(Task.FromResult(1));

            _paymentModelMock.Setup(x => x.CreatePayments(paymentModels)).Returns(new List<Accounts.Models.PaymentModel>());

            _paymentModelMock.Setup(x => x.CreatePaymentSummary(It.IsAny<List<Accounts.Models.PaymentModel>>(),It.IsAny<Accounts.Models.UserModel>())).Returns(new PaymentViewModel());

            _homeController = new HomeController(_paymentRepositoryMock.Object, _userRepositoryMock.Object, _paymentModelMock.Object, _autoMapperMock.Object);
        }

        [Test]
        public async Task ShouldHaveIndexView()
        {
            Setup();

            var viewResult = await _homeController.Index(It.IsAny<Guid>()) as ViewResult;

            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [Test]
        public async Task ShouldHaveUserModel()
        {
            Setup();

            var viewResult = await _homeController.Index(It.IsAny<Guid>()) as ViewResult;

            Assert.IsInstanceOf(typeof(HomeControllerModel), viewResult.Model);
        }


        [Test]
        public async Task ShouldHavePaymentViewModelWithValuesSet()
        {
            Setup();

            var viewResult = await _homeController.GetPaymentSummaryById(_guid);

            var paymentViewModel = viewResult.Data as PaymentViewModel;

            Assert.IsNotNull(paymentViewModel);
        }

        [Test]
        public async Task ShouldReturnGuidWhenAddingPayment()
        {
            Setup();
            var viewResult = await _homeController.AddPayment(It.IsAny<Accounts.Models.PaymentModel>());
            var paymentViewModel = Guid.Parse(viewResult.Data.ToString());

            Assert.That(paymentViewModel, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task ShouldReturnRowsAffectedWhenUpdatingPayment()
        {
            Setup();
            var viewResult = await _homeController.UpdatePayment(It.IsAny<Accounts.Models.PaymentModel>());
            var paymentViewModel = int.Parse(viewResult.Data.ToString());

            Assert.That(paymentViewModel, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnRowsAffectedWhenDeletingPayment()
        {
            Setup();
            var viewResult = await _homeController.DeletePayment(It.IsAny<Guid>());
            var paymentViewModel = int.Parse(viewResult.Data.ToString());

            Assert.That(paymentViewModel, Is.Not.EqualTo(0));
        }
    }
}