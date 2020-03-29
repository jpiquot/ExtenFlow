using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Security.DaprStore.Actors;
using ExtenFlow.Security.Users;

using FluentAssertions;

using Microsoft.AspNetCore.Identity;

using Moq;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Security.DaprStoreTests
{
    public class UserActorTest
    {
        private async Task<UserActor> CreateUserActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(UserActor));
            UserActor actorFactory(ActorService service, ActorId id) =>
                new UserActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            UserActor actor = actorFactory(actorService, new ActorId(id));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }

        [Fact]
        public Task CreateUser_Initialized() => CreateUserActor(new Mock<IActorStateManager>().Object, "test");

        [Fact]
        public async Task CreateUser_ExpectSetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.Create(user);
            result.Succeeded.Should().BeTrue();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task CreateExistingUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.Create(user);

            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            IdentityError error = new IdentityErrorDescriber().DuplicateUserName(user.Id);
            IdentityError resultError = result.Errors.First();
            resultError.Code.Should().Be(error.Code);
            resultError.Description.Should().Be(error.Description);
        }

        [Fact]
        public async Task GetUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);
            await Invoking(async () => await testDemoActor.GetUser())
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetUser_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user))
                .Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            User state = await testDemoActor.GetUser();
            state.Id.Should().Be(user.Id);
            state.UserName.Should().Be(user.UserName);
            state.NormalizedUserName.Should().Be(user.NormalizedUserName);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task GetUserOnDeleteUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.SetDeleted();
            result.Succeeded.Should().Be(true);

            await Invoking(async () => await testDemoActor.GetUser())
                .Should()
                .ThrowAsync<KeyNotFoundException>();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserWithInvalidId_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            await Invoking(async () => await testDemoActor.Update(new User { Id = "other user", UserName = "User name", NormalizedUserName = "username" }))
                .Should()
                .ThrowAsync<InvalidOperationException>();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UpdateUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<User>(null));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            await Invoking(async () => await testDemoActor.Update(new User { Id = "other user", UserName = "User name", NormalizedUserName = "username" }))
                .Should()
                .ThrowAsync<KeyNotFoundException>();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task UpdateUser_ExpectSetStateAsyncNewValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            var modifiedUser = new User { Id = "testuser", UserName = "User name modified", NormalizedUserName = "username modified" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            stateManager.Setup(manager => manager.SetStateAsync("User", modifiedUser, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.Update(modifiedUser);
            result.Succeeded.Should().BeTrue();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task SetUserName_ExpectSetStateAsyncNewValue()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            string newName = "new user name";
            await testDemoActor.SetUserName(newName);
            user.UserName.Should().Be(newName);

            stateManager.VerifyAll();
        }

        [Theory]
        [InlineData("new user Name")]
        [InlineData("very long new user Name 123456789 @!)ази-")]
        public async Task SetNormalizedUserName_ExpectSetStateAsyncNewValue(string name)
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            await testDemoActor.SetNormalizedUserName(name);
            user.NormalizedUserName.Should().Be(name);

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task DeleteUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<User>(null));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            await Invoking(async () => await testDemoActor.SetDeleted())
                .Should()
                .ThrowAsync<KeyNotFoundException>();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task DeleteUser_ExpectSetStateAsyncNull()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            stateManager.Setup(manager => manager.SetStateAsync<User>("User", null, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.SetDeleted();
            result.Succeeded.Should().Be(true);

            stateManager.VerifyAll();
        }
    }
}