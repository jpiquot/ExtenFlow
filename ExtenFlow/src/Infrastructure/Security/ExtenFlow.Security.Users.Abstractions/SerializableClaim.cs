using System.Collections.Generic;

namespace ExtenFlow.Security.Users
{
    /// <summary>
    /// Represents a serialized version for the claim.
    /// </summary>
    public class SerializableClaim
    {
        /// <summary>
        /// Gets or sets the claim subject.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the claim issuer.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Gets or sets the claim original issuer for the claim.
        /// </summary>
        public string? OriginalIssuer { get; set; }

        /// <summary>
        /// Gets or sets the claim properties.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public KeyValuePair<string?, string?>[]? Properties { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Gets or sets the claim type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim value.
        /// </summary>
        public string? ValueType { get; set; }
    }
}