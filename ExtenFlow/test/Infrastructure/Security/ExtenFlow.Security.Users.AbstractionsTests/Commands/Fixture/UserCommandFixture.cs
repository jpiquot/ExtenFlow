#pragma warning disable CS0612 // Type or member is obsolete

using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Security.Users.Commands;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class UserCommandFixture<T> : CommandFixture<T> where T : UserCommand
    {
    }

    public class UserCommandTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "AGGR. Id@", "User @ ID", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}