using System;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Message dispatcher Actor
    /// </summary>
    public interface IBaseActor<T> : IActor
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <returns></returns>
        Task<T> GetStateValue();

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