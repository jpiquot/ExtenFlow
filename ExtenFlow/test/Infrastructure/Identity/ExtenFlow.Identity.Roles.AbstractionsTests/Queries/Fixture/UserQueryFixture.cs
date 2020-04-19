#pragma warning disable CS0612 // Type or member is obsolete

using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Identity.Queries;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class UserQueryFixture<TR, TQ> : QueryFixture<TR, TQ> where TQ : UserQuery<TR>
    {
    }

    public class UserQueryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "AGGR. Id@", "User @ ID", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}