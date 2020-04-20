#pragma warning disable CS0612 // Type or member is obsolete

using System;
using System.Collections;
using System.Collections.Generic;
using ExtenFlow.Identity.Roles.Commands;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class RoleCommandFixture<T> : CommandFixture<T> where T : RoleCommand
    {
    }

    public class RoleCommandTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "Role Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "AGGR. Id@", "Role @ ID", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}