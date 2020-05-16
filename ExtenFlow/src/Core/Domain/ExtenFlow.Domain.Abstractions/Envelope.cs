using System;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.Domain
{
    /// <summary>
    /// Envelope for sending messages
    /// </summary>
    public class Envelope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException">message</exception>
        /// <exception cref="System.ArgumentException">message</exception>
        public Envelope(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            string? typeName = message.GetType().AssemblyQualifiedName;
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException(Properties.Resources.UnkownType, nameof(message));
            }
            ContentType = typeName;
            Content = JsonSerializer.Serialize(message, MessageType, new JsonSerializerOptions { IgnoreReadOnlyProperties = true });
        }

        /// <summary>
        /// Gets the serialized content.
        /// </summary>
        /// <value>The content as a Json string.</value>
        public string Content { get; private set; }

        /// <summary>
        /// Gets the type of the serialized content.
        /// </summary>
        /// <value>Assembly qualified name of the content.</value>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonIgnore]
        public object Message => JsonSerializer.Deserialize(Content, MessageType);

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
#pragma warning disable CS8603 // Possible null reference return.

        [JsonIgnore]
        public Type MessageType => Type.GetType(ContentType, true, false);
    }
}