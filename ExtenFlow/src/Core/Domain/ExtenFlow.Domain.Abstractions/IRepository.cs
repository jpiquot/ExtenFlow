using System.Threading.Tasks;

namespace ExtenFlow.Domain
{
    /// <summary>
    /// Interface IRepository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Reads the data from the storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> GetData<T>(string name);

        /// <summary>
        /// Determines whether the specified storage name has data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> HasData(string name);

        /// <summary>
        /// Removes the data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task.</returns>
        Task RemoveData(string name);

        /// <summary>
        /// Writes the data to the storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Task.</returns>
        Task SetData<T>(string name, T value);

        /// <summary>
        /// Tries the get data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;ConditionalValue&lt;T&gt;&gt;.</returns>
        Task<(bool succeded, T state)> TryGetData<T>(string name);
    }
}