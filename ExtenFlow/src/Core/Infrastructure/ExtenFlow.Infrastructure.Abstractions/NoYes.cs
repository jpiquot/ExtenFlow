namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Class NoYes. Implements the <see cref="ExtenFlow.Infrastructure.DomainEnumeration"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.DomainEnumeration"/>
    public class NoYes : DomainEnumeration
    {
        /// <summary>
        /// The no
        /// </summary>
        public static readonly NoYes No = new NoYes(0, nameof(No));

        /// <summary>
        /// The yes
        /// </summary>
        public static readonly NoYes Yes = new NoYes(1, nameof(Yes));

        /// <summary>
        /// Initializes a new instance of the <see cref="NoYes"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public NoYes(int id, string name)
            : base(id, name)
        {
        }
    }
}