using System;

using ExtenFlow.Infrastructure;

namespace ExtenFlow.Messages.Brighter.Requests
{
    /// <summary>
    /// Class BrighterCommand. Implements the <see cref="Paramore.Brighter.ICommand"/>
    /// </summary>
    /// <seealso cref="Paramore.Brighter.ICommand"/>
    public class BrighterCommand : Paramore.Brighter.ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrighterCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentNullException">command</exception>
        public BrighterCommand(ICommand command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Id = command.Id.ToGuidOrDefault() ?? Guid.NewGuid();
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}