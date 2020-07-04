using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ClientIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNameValidator"/> class.
        /// </summary>
        public ClientIdValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 1, 22, false)
        {
        }
    }
}