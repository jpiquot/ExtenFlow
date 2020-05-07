using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramore.Brighter;

namespace ExtenFlow.Messages.BrighterBroker
{
    /// <summary>
    /// Class DaprActorRequestHandler. Implements the <see cref="Paramore.Brighter.RequestHandlerAsync{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Paramore.Brighter.RequestHandlerAsync{T}"/>
    public class DaprActorRequestHandler<T> : RequestHandlerAsync<T>
        where T : class, IRequest
    {
    }
}