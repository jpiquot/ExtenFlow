using System;

using IdentityServer4.Models;

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Create new client command
    /// </summary>
    /// <seealso cref="ClientCommand"/>
    public class AddNewClient : ClientCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewClient"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public AddNewClient()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewClient"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The client name.</param>
        /// <param name="description">The client description.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public AddNewClient(string aggregateId, string name, string? description, string userId, string? concurrencyCheckStamp = null, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="concurrencyCheckStamp"></param>
        public AddNewClient(Client client, string userId, string? concurrencyCheckStamp = null)
            : this((client == null) ? throw new ArgumentNullException(nameof(client)) : client.ClientId, client.ClientName, client.Description, userId, concurrencyCheckStamp)
        {
        }

        /// <summary>
        /// Gets the new client normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string? Description { get; }

        /// <summary>
        /// Gets the new client name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }
    }
}