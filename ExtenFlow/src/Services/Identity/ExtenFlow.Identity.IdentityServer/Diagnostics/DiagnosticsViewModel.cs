using IdentityModel;

using Microsoft.AspNetCore.Authentication;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Text;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class DiagnosticsViewModel.
    /// </summary>
    public class DiagnosticsViewModel
    {
        private const string _clientList = "client_list";

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsViewModel"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            AuthenticateResult = result;

            if (result?.Properties.Items.ContainsKey(_clientList) == true)
            {
                string? encoded = result?.Properties.Items[_clientList];
                byte[]? bytes = Base64Url.Decode(encoded);
                string? value = Encoding.UTF8.GetString(bytes);

                Clients = JsonConvert.DeserializeObject<string[]>(value);
            }
        }

        /// <summary>
        /// Gets the authenticate result.
        /// </summary>
        /// <value>The authenticate result.</value>
        public AuthenticateResult AuthenticateResult { get; }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <value>The clients.</value>
        public IEnumerable<string> Clients { get; } = new List<string>();
    }
}