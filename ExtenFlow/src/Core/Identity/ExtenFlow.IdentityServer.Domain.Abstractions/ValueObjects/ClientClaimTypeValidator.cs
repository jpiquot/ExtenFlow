﻿using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimTypeValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ClientClaimTypeValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientClaimTypeValidator"/> class.
        /// </summary>
        public ClientClaimTypeValidator(string? parentName = null, string? propertyName = null) : base(parentName, propertyName, false, 1, 256, false)
        {
        }
    }
}