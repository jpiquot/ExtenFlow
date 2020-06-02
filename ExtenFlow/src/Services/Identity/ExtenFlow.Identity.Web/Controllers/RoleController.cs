using System;
using System.Collections.Generic;
using System.Linq;

using ExtenFlow.Identity.Roles.Application.Queries;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.Web.Controllers
{
    /// <summary>
    /// Class RoleController. Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public RoleController(ILogger<RoleController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;RoleDetails&gt;.</returns>
        [HttpGet]
#pragma warning disable CA1822 // Mark members as static
        public IEnumerable<RoleDetails> Get()
        {
            return new RoleDetails[]
            {
                new RoleDetails("role1","Role 1","ROLE1","stamp1"),
                new RoleDetails("role2","Role 2","ROLE2","stamp2"),
                new RoleDetails("role3","Role 3","ROLE3","stamp3"),
                new RoleDetails("role4","Role 4","ROLE4","stamp4"),
                new RoleDetails("role5","Role 5","ROLE5","stamp5")
            };
        }
    }
}