﻿using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class AggregateIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateIdValidator"/> class.
        /// </summary>
        public AggregateIdValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 1, 1024, false)
        {
        }
    }
}