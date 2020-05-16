using System;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Message dispatcher Actor
    /// </summary>
    public interface IBaseActor : IActor
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <returns>The state instance</returns>
        Task<object> GetStateValue();

        /// <summary>
        /// Determines whether this instance is initialized.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsInitialized();

        /// <summary>
        /// Registers the reminder.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <param name="state">The reminder state</param>
        /// <param name="reminderName">The reminder name</param>
        Task RegisterReminder(TimeSpan dueTime, TimeSpan period, byte[]? state = null, string? reminderName = null);
    }
}