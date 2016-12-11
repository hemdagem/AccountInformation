using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.ModelBuilders;
using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Accounts.Database.DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PaymentRepositoryTests
    {
        private Mock<IDbConnectionFactory> _dataAccessMock;
        private List<PaymentModel> _paymentModelList;
        private Guid _guid;
        private PaymentRepository _paymentRepository;
        private PaymentModel _paymentModel;
        private Mock<IPaymentModelBuilder> _paymentModelBuilderMock;

        private void Setup()
        {
            _guid = Guid.NewGuid();
            _dataAccessMock = new Mock<IDbConnectionFactory>();
            _paymentModelBuilderMock = new Mock<IPaymentModelBuilder>();

            _paymentModelList = new List<PaymentModel>
            {
                new PaymentModel
                {
                    Amount = 28,
                    Id = _guid,
                    PaidYearly = true,
                    Paid = true,
                    Date = DateTime.Today,
                    Title = "Title"
                }
            };

            _paymentModel = new PaymentModel
            {
                Amount = 20,
                Date = DateTime.UtcNow,
                IncomeId = Guid.NewGuid(),
                PaidYearly = true
            };

            _paymentRepository = new PaymentRepository(_dataAccessMock.Object, _paymentModelBuilderMock.Object);
        }

        [Test]
        public async Task ShouldReturnDataWhenUserIsFound()
        {
            //given
            Setup();

            var dictionary = new Dictionary<string, object>
            {
                {"Id", _guid},
                {"Title", "Title"},
                {"Amount", 28},
                {"PaidYearly", true},
                {"Paid", true},
                {"Date", DateTime.Today},
                {"Recurring", true},
                {"PayDay", 6}
            };
            var reader = DataReaderTestHelper.Reader(dictionary);

            _dataAccessMock.Setup(x => x.ExecuteReader("up_GetPaymentsById", It.IsAny<SqlParameter>())).Returns(Task.FromResult(reader));
            _paymentModelBuilderMock.Setup(x => x.Build(reader)).Returns(_paymentModelList);
            //when
            var paymentsById = await _paymentRepository.GetPaymentsById(It.IsAny<Guid>());

            //then
            Assert.That(paymentsById.Count, Is.EqualTo(1));
            Assert.That(paymentsById[0].Amount, Is.EqualTo(_paymentModelList[0].Amount));
            Assert.That(paymentsById[0].Id, Is.EqualTo(_paymentModelList[0].Id));
            Assert.That(paymentsById[0].PaidYearly, Is.EqualTo(_paymentModelList[0].PaidYearly));
            Assert.That(paymentsById[0].Paid, Is.EqualTo(_paymentModelList[0].Paid));
            Assert.That(paymentsById[0].Date, Is.EqualTo(_paymentModelList[0].Date));
            Assert.That(paymentsById[0].Title, Is.EqualTo(_paymentModelList[0].Title));
        }

        [Test]
        public void GetPaymentsById_Should_throw_null_exception_when_no_data_is_found()
        {
            //given
            Setup();

            _dataAccessMock.Setup(x => x.ExecuteReader("up_GetPaymentsById", It.IsAny<SqlParameter>())).Returns(Task.FromResult<IDataReader>(null));
            //when

            //then
            _paymentModelBuilderMock.Verify(x=>x.Build(It.IsAny<SqlDataReader>()),Times.Never);
            Assert.Throws<NullReferenceException>(async () => await _paymentRepository.GetPaymentsById(It.IsAny<Guid>()));
        }

        [Test]
        public async Task ShouldReturnNewGuidWhenPaymentAdded()
        {
            //given
            Setup();
            _dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddPayment", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(),
                It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(_guid));

            //when
            var paymentsById = await _paymentRepository.AddPayment(_paymentModel);

            //then
            Assert.That(paymentsById, Is.EqualTo(_guid));
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ShouldThrowExceptionWhenNothingisReturned()
        {
            //given
            Setup();

            var guidSqlParameter = new SqlParameter("IncomeId", _paymentModel.Id);
            var amountSqlParameter = new SqlParameter("Amount", _paymentModel.Amount);
            var dateSqlParameter = new SqlParameter("Amount", _paymentModel.Date);
            var paidYearlySqlParameter = new SqlParameter("Amount", _paymentModel.Date);
            var paymentTypeSqlParameter = new SqlParameter("Amount", _paymentModel.Date);

            _dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddPayment", guidSqlParameter, amountSqlParameter, dateSqlParameter, paidYearlySqlParameter, paymentTypeSqlParameter)).Returns(Task.FromResult(Guid.Empty));

            //when
            await _paymentRepository.AddPayment(_paymentModel);
        }


        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public async Task ShouldReturnRowsAffectedWhenDeletingPayment(int rowsAffected)
        {
            //given
            Setup();
            _dataAccessMock.Setup(x => x.ExecuteScalar<int>("up_DeletePayment", It.IsAny<SqlParameter>())).Returns(Task.FromResult(rowsAffected));

            var deletePayment = await _paymentRepository.DeletePayment(_guid);

            Assert.That(deletePayment, Is.EqualTo(rowsAffected));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public async Task ShouldReturnRowsAffectedWhenUpdatingPayment(int rowsAffected)
        {
            //given
            Setup();
            _dataAccessMock.Setup(x => x.ExecuteScalar<int>("up_UpdatePayment", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(rowsAffected));

            var updatePayment = await _paymentRepository.UpdatePayment(_paymentModel);

            Assert.That(updatePayment, Is.EqualTo(rowsAffected));
        }
    }
}