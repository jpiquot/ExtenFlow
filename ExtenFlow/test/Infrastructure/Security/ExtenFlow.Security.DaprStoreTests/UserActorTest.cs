using System.Collections.Generic;
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
        private UserActor CreateUserActor(IActorStateManager actorStateManager, string id)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(UserActor));
            UserActor actorFactory(ActorService service, ActorId id) =>
                new UserActor(service, id, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            UserActor actor = actorFactory(actorService, new ActorId(id));
            return actor;
        }

        [Fact]
        public void CreateUser_Initialized() => CreateUserActor(new Mock<IActorStateManager>().Object, "test");

        [Fact]
        public async Task CreateUser_ExpectSetStateAsync()
        {
            //Arrange
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.SetStateAsync("User", user, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = CreateUserActor(stateManager.Object, user.Id);
            //Act

            IdentityResult result = await testDemoActor.Create(user);
            result.Succeeded.Should().Be(true);

            //Assert
            stateManager.VerifyAll();
        }

        [Fact]
        public async Task GetUninitializedUser_ThrowsException()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            UserActor testDemoActor = CreateUserActor(stateManager.Object, user.Id);
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
            UserActor testDemoActor = CreateUserActor(stateManager.Object, user.Id);

            // OnActivated is not called in test object. A read call needs to be done manually.
            await testDemoActor.Read();

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
            UserActor testDemoActor = CreateUserActor(stateManager.Object, user.Id);

            // OnActivated is not called in test object. A read call needs to be done manually.
            await testDemoActor.Read();

            IdentityResult result = await testDemoActor.Delete();
            result.Succeeded.Should().Be(true);

            await Invoking(async () => await testDemoActor.GetUser())
                .Should()
                .ThrowAsync<KeyNotFoundException>();

            stateManager.VerifyAll();
        }

        [Fact]
        public async Task DeleteUser_ExpectSetStateAsync()
        {
            var stateManager = new Mock<IActorStateManager>();
            var user = new User { Id = "testuser", UserName = "User name", NormalizedUserName = "username" };
            stateManager.Setup(manager => manager.GetStateAsync<User>("User", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(user));
            stateManager.Setup(manager => manager.SetStateAsync<User>("User", null, It.IsAny<CancellationToken>())).Verifiable();
            UserActor testDemoActor = CreateUserActor(stateManager.Object, user.Id);

            // OnActivated is not called in test object. A read call needs to be done manually.
            await testDemoActor.Read();

            IdentityResult result = await testDemoActor.Delete();
            result.Succeeded.Should().Be(true);

            stateManager.VerifyAll();
        }
    }
}