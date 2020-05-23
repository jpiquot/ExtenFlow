namespace ExtenFlow.Infrastructure.Validators
{
    /// <summary>
    /// Class ConcurrencyStampValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ConcurrencyStampValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyStampValidator"/> class.
        /// </summary>
        public ConcurrencyStampValidator() : base(false, 22, 22, false)
        {
        }
    }
}