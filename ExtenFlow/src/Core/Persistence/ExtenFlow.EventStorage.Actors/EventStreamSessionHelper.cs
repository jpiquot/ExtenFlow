namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStreamSessionHelper.
    /// </summary>
    public static class EventStreamSessionHelper
    {
        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <param name="sessionId">The version.</param>
        /// <returns>System.String.</returns>
        public static string GetSessionId(string streamId, long sessionId)
            => $"{streamId}-({sessionId})";
    }
}