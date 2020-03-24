using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Security.DaprStore.Actors;
using ExtenFlow.Security.Users;

using FluentAssertions;

using Moq;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Security.DaprStoreTests
{
    public class UserActorTest
    {
        private UserActor CreateUserActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(UserActor));
            Func<ActorService, ActorId, UserActor> actorFactory
                = (service, id) =>
                new UserActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            var actor = actorFactory.Invoke(actorService, new ActorId(id));
            return actor;
        }

        [Fact]
        public void Constructor_Initialize() => CreateUserActor(new Mock<IActorStateManager>().Object, "test");

        [Fact]
        public async Task CreateUser_Expect_SetStateAsync()
        {
            //Arrange
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            var testDemoActor = CreateUserActor(stateManager.Object, user.Id);
            //Act

            var result = await testDemoActor.Create(user);
            result.Succeeded.Should().Be(true);

            //Assert
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task GetUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            var testDemoActor = CreateUserActor(stateManager.Object, user.Id);
            await Invoking(async () => await testDemoActor.GetUser())
                .Should()
                .ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetUser_Expect_GetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user))
                .Verifiable();
            var testDemoActor = CreateUserActor(stateManager.Object, user.Id);

            // OnActivated is not called in test object. A read call needs to be done manually.
            await testDemoActor.Read();

            User state = await testDemoActor.GetUser();
            state.Id.Should().Be(user.Id);

            stateManager.VerifyAll();
        }
    }
}