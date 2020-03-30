using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection state
    /// </summary>
    public class UserLoginsCollectionState
    {
        private Dictionary<string, Dictionary<string, Guid>>? _loginProviders;

        /// <summary>
        /// Gets the login providers.
        /// </summary>
        /// <value>The login providers.</value>
        internal Dictionary<string, Dictionary<string, Guid>> LoginProviders => _loginProviders ?? (_loginProviders = new Dictionary<string, Dictionary<string, Guid>>());

        internal Dictionary<string, Guid> GetProviderKeys(string loginProvider)
        {
            if (!LoginProviders.TryGetValue(loginProvider, out Dictionary<string, Guid>? values))
            {
                values = new Dictionary<string, Guid>();
                LoginProviders.Add(loginProvider, values);
            }
            return values;
        }

        internal void Add(string loginProvider, string providerKey, Guid userId)
        {
            Dictionary<string, Guid> keys = GetProviderKeys(loginProvider);
            if (!keys.TryGetValue(providerKey, out Guid user))
            {
                keys.Add(providerKey, userId);
            }
            else
            {
                if (userId != user)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                      Resource.DuplicateUserForLogin,
                                                                      loginProvider,
                                                                      providerKey,
                                                                      user,
                                                                      userId));
                }
            }
        }

        internal void Remove(string loginProvider, string providerKey, Guid userId)
        {
            Dictionary<string, Guid> keys = GetProviderKeys(loginProvider);
            if (!keys.TryGetValue(providerKey, out Guid user))
            {
                return;
            }
            if (userId != user)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                  Resource.DuplicateUserForLogin,
                                                                  loginProvider,
                                                                  providerKey,
                                                                  user,
                                                                  userId));
            }
        }
    }
}