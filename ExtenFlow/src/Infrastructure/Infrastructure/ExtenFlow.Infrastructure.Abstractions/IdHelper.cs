using System;
using System.Globalization;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Helper methods to manage Ids.
    /// </summary>
    public static class IdHelper
    {
        /// <summary>
        /// Generates the identifier.
        /// </summary>
        /// <param name="proposedId">The proposed identifier.</param>
        /// <param name="checkExist">The check exist.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>System.String.</returns>
        public static string GenerateId(this string proposedId, Func<string, bool>? checkExist = null, int maxLength = 22)
        {
            if (string.IsNullOrWhiteSpace(proposedId))
            {
                if (maxLength < 22)
                {
                    string id;
                    do
                    {
                        id = Guid
                            .NewGuid()
                            .ToBase64String()
                            .Substring(22 - maxLength);
                    }
                    while (checkExist != null && checkExist(id));
                    return id;
                }
                else
                {
                    return Guid.NewGuid().ToBase64String();
                }
            }
            if (proposedId.Length > maxLength)
            {
                proposedId = proposedId.Substring(0, maxLength);
            }
            if (checkExist == null || !checkExist(proposedId))
            {
                return proposedId;
            }
            proposedId = proposedId.Substring(0, Math.Min(maxLength - 1, proposedId.Length));
            for (int i = 2; i < 10; i++) // try to generate an Id with an index if the id already exists
            {
                string id = proposedId + i.ToString(CultureInfo.InvariantCulture);
                if (checkExist == null || !checkExist(id))
                {
                    return id;
                }
            }
            return string.Empty.GenerateId(); // Use a Guid to create Id.
        }
    }
}