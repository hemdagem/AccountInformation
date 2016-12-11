using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Accounts.Database.DataAccess.Interfaces;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PaymentTypeRepositoryTests
    {
        [Test]
        public async Task ShouldGetPaymentTypes()
        {
            //given
            var dictionary = new Dictionary<string, object>
            {
                {"Id", Guid.NewGuid()},
                {"Title", "Name"},
            };


            var helper = new Mock<IDbConnectionFactory>();
            helper.Setup(x => x.ExecuteReader("up_GetPaymentTypes")).Returns(Task.FromResult(DataReaderTestHelper.Reader(dictionary)));

            //when
            var repository = new PaymentTypeRepository(helper.Object);
            var resultsModels = await repository.GetPaymentTypes();

            //then
            Assert.That(resultsModels, Is.Not.Null);
            Assert.That(resultsModels.First().Text, Is.EqualTo(dictionary["Title"].ToString()));
            Assert.That(resultsModels.First().Value, Is.EqualTo(dictionary["Id"].ToString()));

        } 
        
        [Test]
        public async Task ShouldGetPaymentType()
        {
            //given

            var dictionary = new Dictionary<string, object>
            {
                {"Id", Guid.NewGuid()},
                {"Title", "Name"},
            };

            var helper = new Mock<IDbConnectionFactory>();
            helper.Setup(x => x.ExecuteReader("up_GetPaymentType",It.IsAny<SqlParameter>())).Returns(Task.FromResult(DataReaderTestHelper.Reader(dictionary)));

            //when
            var repository = new PaymentTypeRepository(helper.Object);
            var resultsModels = await repository.GetPaymentType(It.IsAny<Guid>());

            //then
            Assert.That(resultsModels, Is.Not.Null);
            Assert.That(resultsModels.Title, Is.EqualTo(dictionary["Title"].ToString()));
            Assert.That(resultsModels.Id, Is.EqualTo(dictionary["Id"]));

        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public async Task ShouldReturnRowsAffectedWhenUpdatingPaymentType(int rowsAffected)
        {
            var dataAccessMock = new Mock<IDbConnectionFactory>();

            var model = new PaymentTypes();

            dataAccessMock.Setup(x => x.ExecuteScalar<int>("up_UpdatePaymentType", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(rowsAffected);

            var paymentRepository = new PaymentTypeRepository(dataAccessMock.Object);

            var updatePaymentType = await paymentRepository.UpdatePaymentType(model);

            Assert.That(updatePaymentType, Is.EqualTo(rowsAffected));
        } 
        
        [Test]
        public async Task ShouldReturnRowsAffectedWhenAddingPaymentType()
        {
            var dataAccessMock = new Mock<IDbConnectionFactory>();
            var id = Guid.NewGuid();
            var model = new PaymentTypes();

            dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddPaymentType", It.IsAny<SqlParameter>())).Returns(id);

            var paymentRepository = new PaymentTypeRepository(dataAccessMock.Object);

            var updatePaymentType = await paymentRepository.AddPaymentType(model);

            Assert.That(updatePaymentType, Is.EqualTo(id));
        }

    }
}
