using System;

namespace ExtenFlow.EventBus.Brighter.Requests
{
    /// <summary>
    /// Class BrighterRequest. Implements the <see cref="Paramore.Brighter.IRequest"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Paramore.Brighter.IRequest"/>
    public class BrighterRequest<T> : Paramore.Brighter.IRequest
        where T : ExtenFlow.Messages.IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrighterRequest{T}"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public BrighterRequest(T request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Guid Id { get; set; }
        private T Request { get; }
    }
}