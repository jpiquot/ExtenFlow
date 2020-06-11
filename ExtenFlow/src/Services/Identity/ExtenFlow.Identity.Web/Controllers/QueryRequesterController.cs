using System;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtenFlow.Identity.Web.Controllers
{
    /// <summary>
    /// Class QueryRequesterController. Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    [Route("api/[controller]")]
    [ApiController]
    public class QueryRequesterController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryRequester _queryRequester;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRequesterController"/> class.
        /// </summary>
        /// <param name="queryRequester">The query requester.</param>
        /// <param name="httpContextAccessor">The context</param>
        /// <exception cref="System.ArgumentNullException">queryRequester</exception>
        public QueryRequesterController(IQueryRequester queryRequester, IHttpContextAccessor httpContextAccessor)
        {
            _queryRequester = queryRequester ?? throw new ArgumentNullException(nameof(queryRequester));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Asks the specified envelope.
        /// </summary>
        /// <param name="query">The envelope.</param>
        /// <returns>Envelope.</returns>
        public async Task<Envelope> Ask(Envelope query)
        {
            object result = await _queryRequester.Ask(
                (query?.Message as IQuery)
                ?? throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    ExtenFlow.Messages.Properties.Resources.InvalidQuery,
                    query?.MessageType.Name, nameof(QueryRequesterController) + "." + nameof(Ask)
                    ),
                    nameof(query)));
            return new Envelope(result);
        }
    }
}