using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Security.Users.Commands;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class RegisterNewUserTest : IClassFixture<UserCommandFixture<RegisterNewUser>>
    {
        private UserCommandFixture<RegisterNewUser> RegisterNewUserFixture { get; }

        public RegisterNewUserTest(UserCommandFixture<RegisterNewUser> registerNewUserFixture)
        {
            RegisterNewUserFixture = registerNewUserFixture;
        }

        [Fact]
        public void CreateRegisterNewUser_EmptyMessageIdShouldThrowException()
            => Invoking(() => new RegisterNewUser("Aggr. Id", "pass", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRegisterNewUser_DefaultMessageShouldHaveAMessageId()
            => new RegisterNewUser("aggr id", "pass").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRegisterNewUser_DefaultMessageShouldHaveACorrelationId()
            => new RegisterNewUser("aggr id", "pass").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRegisterNewUser_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new RegisterNewUser("Aggr. Id", "pass", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRegisterNewUser_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new RegisterNewUser("Aggr. Id", "pass", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RegisterNewUserTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewUserFixture.CheckMessageNewtonSoftSerialization(new RegisterNewUser(aggregateId, password, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RegisterNewUserTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewUserFixture.CheckMessageJsonSerialization(new RegisterNewUser(aggregateId, password, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RegisterNewUserTestDate))]
        public void CreateRegisterNewUser_CheckState(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewUserFixture.CheckMessageState(new RegisterNewUser(aggregateId, password, userId, correlationId, messageId, dateTime), "User", aggregateId, userId, correlationId, messageId, dateTime);
    }

    public class RegisterNewUserTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "passWord @ 123", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "passWord123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}