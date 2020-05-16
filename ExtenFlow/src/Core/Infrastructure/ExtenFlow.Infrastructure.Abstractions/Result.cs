using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Class Result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        public Result(bool failed, IList<string> messages)
        {
            if (failed && !(messages?.Any() == true))
            {
                throw new ArgumentException(Properties.Resources.MissingFailureMessages, nameof(messages));
            }
            Messages = messages ?? new List<string>();
            HasFailed = failed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentException">messages</exception>
        public Result(bool failed, params string[] messages)
        {
            if (failed && !(messages.Any() == true))
            {
                throw new ArgumentException(Properties.Resources.MissingFailureMessages, nameof(messages));
            }
            Messages = messages;
            HasFailed = failed;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Result"/> is failed.
        /// </summary>
        /// <value><c>true</c> if failed; otherwise, <c>false</c>.</value>
        public bool HasFailed { get; }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public IList<string> Messages { get; }

        /// <summary>
        /// Faileds the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>Result.</returns>
        public static Result Failed(IList<string> messages)
            => new Result(true, messages);

        /// <summary>
        /// Faileds the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>Result.</returns>
        public static Result Failed(params string[] messages)
            => new Result(true, messages);

        /// <summary>
        /// Faileds the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>Result.</returns>
        public static Result<T> Failed<T>(params string[] messages)
            => new Result<T>(messages);

        /// <summary>
        /// Faileds the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>Result.</returns>
        public static Result<T> Failed<T>(IList<string> messages)
            => new Result<T>(messages);

        /// <summary>
        /// Successes the specified messages.
        /// </summary>
        /// <returns>Result.</returns>
        public static Result Succeeded()
            => new Result(false);

        /// <summary>
        /// Successes the specified messages.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Result.</returns>
        public static Result<T> Succeeded<T>(T value)
            => new Result<T>(value, false);
    }

    /// <summary>
    /// Class Result. Implements the <see cref="ExtenFlow.Infrastructure.Result"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Infrastructure.Result"/>
    public class Result<T> : ValuedResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public Result(T value, bool failed, IList<string> messages) : base(value, failed, messages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public Result(T value, bool failed, params string[] messages) : this(value, failed, (IList<string>)messages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public Result(IList<string> messages) : base(null, true, messages)
        {
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                if (HasFailed)
                {
                    throw new InvalidOperationException(Properties.Resources.FailedResultValueAccess);
                }
                return (T)ValueObject ?? throw new InvalidOperationException(Properties.Resources.ResultValueIsNull);
            }
        }
    }

    /// <summary>
    /// Class Result. Implements the <see cref="ExtenFlow.Infrastructure.Result"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Result"/>
    public class ValuedResult : Result
    {
        /// <summary>
        /// The value
        /// </summary>

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public ValuedResult(object? value, bool failed, IList<string> messages) : base(failed, messages)
        {
            if (value == null)
            {
                if (failed)
                {
                    ValueObject = default;
                }
                else
                {
                    throw new ArgumentNullException(nameof(value));
                }
            }
            ValueObject = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public ValuedResult(object? value, bool failed, params string[] messages) : this(value, failed, (IList<string>)messages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public ValuedResult(IList<string> messages) : base(true, messages)
        {
            ValueObject = default;
        }

        /// <summary>
        /// Gets the value object.
        /// </summary>
        /// <value>The value object.</value>
        protected object? ValueObject { get; }
    }
}