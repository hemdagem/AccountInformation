using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Accounts.Database.DataAccess.Interfaces;
using Accounts.Tests.Unit.TestHelpers;
using Moq;
using NUnit.Framework;

namespace Accounts.Tests.Unit.Accounts.Core.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [Test]
        public async Task ShouldGetUsers()
        {
            //given
            var users = new Dictionary<string, object>
            {
                { "Amount", 4},
                { "Id", Guid.NewGuid()},
                { "Name", "Name"}
            };

            var helper = new Mock<IDbConnectionFactory>();
            helper.Setup(x => x.ExecuteReader("up_GetUsers")).Returns(Task.FromResult(DataReaderTestHelper.Reader(users)));

            //when
            var repository = new UserRepository(helper.Object);
            var resultsModels = await repository.GetUsers();

            //then
            Assert.That(resultsModels, Is.Not.Null);
            Assert.That(resultsModels.First().Text, Is.EqualTo(users["Name"].ToString()));
            Assert.That(resultsModels.First().Value, Is.EqualTo(users["Id"].ToString()));
        }

        [Test]
        public async Task ShouldGetUser()
        {
            //given
            var userModel = new Dictionary<string, object>
            {
                { "Amount", 4},
                { "Id", Guid.NewGuid()},
                { "Name", "Name"},
                { "PayDay", 200}
            };


            var helper = new Mock<IDbConnectionFactory>();
            helper.Setup(x => x.ExecuteReader("up_GetUser", It.IsAny<SqlParameter>())).Returns(Task.FromResult(DataReaderTestHelper.Reader(userModel)));

            //when
            var repository = new UserRepository(helper.Object);
            var resultsModels = await repository.GetUser(It.IsAny<Guid>());

            //then
            Assert.That(resultsModels, Is.Not.Null);
            Assert.That(resultsModels.Name, Is.EqualTo(userModel["Name"]));
            Assert.That(resultsModels.Amount, Is.EqualTo(userModel["Amount"]));
            Assert.That(resultsModels.Id, Is.EqualTo(userModel["Id"]));
            Assert.That(resultsModels.PayDay, Is.EqualTo(userModel["PayDay"]));

        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public async Task ShouldReturnRowsAffectedWhenUpdatingUser(int rowsAffected)
        {
            var dataAccessMock = new Mock<IDbConnectionFactory>();

            var model = new UserModel();

            dataAccessMock.Setup(x =>  x.ExecuteScalar<int>("up_UpdateUser", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(rowsAffected));

            var paymentRepository = new UserRepository(dataAccessMock.Object);

            var updateUser = await paymentRepository.UpdateUser(model);

            Assert.That(updateUser, Is.EqualTo(rowsAffected));
        }

        [Test]
        public async Task ShouldReturnRowsAffectedWhenAddingUser()
        {
            var dataAccessMock = new Mock<IDbConnectionFactory>();
            var id = Guid.NewGuid();
            var model = new UserModel();

            dataAccessMock.Setup(x => x.ExecuteScalar<Guid>("up_AddUser", It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>())).Returns(Task.FromResult(id));

            var userRepository = new UserRepository(dataAccessMock.Object);

            var updateUser = await userRepository.AddUser(model);

            Assert.That(updateUser, Is.EqualTo(id));
        }

    }
}
