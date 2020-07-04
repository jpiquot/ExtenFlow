using ExtenFlow.Infrastructure;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class NameValue
    /// </summary>
    public class ClientId : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientName"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public ClientId(string value, string? parentName = null, string? propertyName = null)
            : base(value, new ClientIdValidator(parentName, propertyName))
        {
        }
    }
}