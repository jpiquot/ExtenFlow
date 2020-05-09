using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Users.Actors;
using ExtenFlow.Identity.Users.Models;

using FluentAssertions;

using Microsoft.AspNetCore.Identity;

using Moq;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Security.DaprStoreTests
{
    public class UserActorTest
    {
        [Fact]
        public async Task CreateUser_ExpectSetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.SetUser(user);
            result.Succeeded.Should().BeTrue();

            stateManager.VerifyAll();
        }

        [Fact]
        public Task CreateUser_Initialized() => CreateUserActor(new Mock<IActorStateManager>().Object, Guid.NewGuid());

        [Fact]
        public async Task GetUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username" };
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);
            await Invoking(async () => await testDemoActor.GetUser())
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetUser_ExpectGetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username" };
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
        public async Task SetExistingUser_ExcectConcurrencyStampChange()
        {
            var stateManager = new Mock<IActorStateManager>();
            string concurrencyStamp = Guid.NewGuid().ToString();
            var existingUser = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username", ConcurrencyStamp = concurrencyStamp };
            var newUser = new User { Id = existingUser.Id, UserName = "User name", NormalizedUserName = "username", ConcurrencyStamp = concurrencyStamp };
            stateManager.Setup(manager => manager
                .GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(existingUser));
            stateManager.Setup(manager => manager.SetStateAsync("User", newUser, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, newUser.Id);

            IdentityResult result = await testDemoActor.SetUser(newUser);

            result.Succeeded.Should().BeTrue();
            newUser.Should().NotBe(concurrencyStamp);
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task SetNewUser_ExpectConcurrencyStampChange()
        {
            var stateManager = new Mock<IActorStateManager>();
            string concurrencyStamp = Guid.NewGuid().ToString();
            var user = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username", ConcurrencyStamp = concurrencyStamp };
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            IdentityResult result = await testDemoActor.SetUser(user);
            result.Succeeded.Should().BeTrue();
            user.Should().NotBe(concurrencyStamp);
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task SetUserWithConcurrencyStampMismatch_ReturnsConcurrencyFailure()
        {
            var stateManager = new Mock<IActorStateManager>();
            var existingUser = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username", ConcurrencyStamp = Guid.NewGuid().ToString() };
            var newUser = new User { Id = existingUser.Id, UserName = "User name", NormalizedUserName = "username", ConcurrencyStamp = Guid.NewGuid().ToString() };
            stateManager.Setup(manager => manager
                .GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(existingUser));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, newUser.Id);

            IdentityResult result = await testDemoActor.SetUser(newUser);

            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            IdentityError error = new IdentityErrorDescriber().ConcurrencyFailure();
            IdentityError resultError = result.Errors.First();
            resultError.Code.Should().Be(error.Code);
            resultError.Description.Should().Be(error.Description);
        }

        [Fact]
        public async Task SetUserWithInvalidId_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = Guid.NewGuid(), UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            UserActor testDemoActor = await CreateUserActor(stateManager.Object, user.Id);

            await Invoking(async () => await testDemoActor.SetUser(new User { Id = default, UserName = "User name", NormalizedUserName = "username" }))
                .Should()
                .ThrowAsync<ArgumentOutOfRangeException>();

            stateManager.VerifyAll();
        }

        private async Task<UserActor> CreateUserActor(IActorStateManager actorStateManager, Guid id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(UserActor));
            UserActor actorFactory(ActorService service, ActorId id) =>
                new UserActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            UserActor actor = actorFactory(actorService, new ActorId(id.ToString()));
            MethodInfo OnActivate = actor.GetType().GetMethod("OnActivateAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            await (Task)OnActivate.Invoke(actor, Array.Empty<object>());
            return actor;
        }
    }
}