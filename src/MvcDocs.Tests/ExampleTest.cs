using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace MvcDocs.Tests
{
    [TestFixture]
    public class ExampleTest
    {
        [Test]
        public void OneShouldEqualOne()
        {
            1.Should().Be(1);
        }

        [Test]
        public void ShouldAssertions()
        {
            const string actual = "ABCDEFGHI";
            actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
        }
    }
}
