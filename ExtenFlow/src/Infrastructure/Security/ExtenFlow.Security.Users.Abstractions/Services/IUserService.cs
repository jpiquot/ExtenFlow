namespace ExtenFlow.Security.Users.Services
{
    /// <summary>
    /// Contract that provides an abstraction for common user operations.
    /// </summary>
    public interface IUserService : IUserQueryService, IUserCommandService
    {
    }
}