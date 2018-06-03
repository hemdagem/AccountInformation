using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Accounts.Core.ModelBuilders;
using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Accounts.Database.DataAccess.Interfaces;
using Accounts.Tests.Unit.TestHelpers;
using Moq;
using Xunit;

namespace Accounts.Tests.Unit.Accounts.Core.Repositories
{

    public class PaymentRepositoryTests
    {
        private Mock<IDbConnectionFactory> _dataAccessMock;
        private List<PaymentModel> _paymentModelList;
        private Guid _guid;
        private PaymentRepository _paymentRepository;
        private PaymentModel _paymentModel;
        private Mock<IPaymentModelBuilder> _paymentModelBuilderMock;

        public PaymentRepositoryTests()
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

        [Fact]
        public async Task ShouldReturnDataWhenUserIsFound()
        {
            //given

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
            Assert.Single(paymentsById);
            Assert.Equal(paymentsById[0].Amount, _paymentModelList[0].Amount);
            Assert.Equal(paymentsById[0].Id, _paymentModelList[0].Id);
            Assert.Equal(paymentsById[0].PaidYearly, _paymentModelList[0].PaidYearly);
            Assert.Equal(paymentsById[0].Paid, _paymentModelList[0].Paid);
            Assert.Equal(paymentsById[0].Date, _paymentModelList[0].Date);
            Assert.Equal(paymentsById[0].Title, _paymentModelList[0].Title);
        }
        [Fact]
        public async Task GetPaymentsById_Should_throw_null_exception_when_no_data_is_found()
        {
            //given
            _dataAccessMock.Setup(x => x.ExecuteReader("up_GetPaymentsById", It.IsAny<SqlParameter>())).Returns(Task.FromResult<IDataReader>(null));
            //when

            //then
            _paymentModelBuilderMock.Verify(x => x.Build(It.IsAny<SqlDataReader>()), Times.Never);
            await Assert.ThrowsAsync<NullReferenceException>(() => _paymentRepository.GetPaymentsById(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task ShouldReturnNewGuidWhenPaymentAdded()
        {
            //given
            _dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddPayment", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(),
                It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(_guid));

            //when
            var paymentsById = await _paymentRepository.AddPayment(_paymentModel);

            //then
            Assert.Equal(paymentsById,_guid);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenNothingisReturned()
        {
            //given
            var guidSqlParameter = new SqlParameter("IncomeId", _paymentModel.Id);
            var amountSqlParameter = new SqlParameter("Amount", _paymentModel.Amount);
            var dateSqlParameter = new SqlParameter("Amount", _paymentModel.Date);
            var paidYearlySqlParameter = new SqlParameter("Amount", _paymentModel.Date);
            var paymentTypeSqlParameter = new SqlParameter("Amount", _paymentModel.Date);

            _dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddPayment", guidSqlParameter, amountSqlParameter, dateSqlParameter, paidYearlySqlParameter, paymentTypeSqlParameter)).Returns(Task.FromResult(Guid.Empty));

            //when
            await Assert.ThrowsAsync<NullReferenceException>(() => _paymentRepository.AddPayment(_paymentModel));

        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task ShouldReturnRowsAffectedWhenDeletingPayment(int rowsAffected)
        {
            //given
            _dataAccessMock.Setup(x => x.ExecuteScalar<int>("up_DeletePayment", It.IsAny<SqlParameter>())).Returns(Task.FromResult(rowsAffected));

            var deletePayment = await _paymentRepository.DeletePayment(_guid);

            Assert.Equal(deletePayment,rowsAffected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task ShouldReturnRowsAffectedWhenUpdatingPayment(int rowsAffected)
        {
            //given
            _dataAccessMock.Setup(x => x.ExecuteScalar<int>("up_UpdatePayment", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(rowsAffected));

            var updatePayment = await _paymentRepository.UpdatePayment(_paymentModel);

            Assert.Equal(updatePayment,rowsAffected);
        }
    }
}