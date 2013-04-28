using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MvcDocs.Tests
{
	[TestFixture]
	public class StringExtensionTests
	{
		[Test]
		public void Highlight_WhenTermMatchFound_ShouldSurroundWithSpan()
		{
			"foo bar baz".Highlight(new[] { "bar" }).Should().Be("foo <span class=\"hl\">bar</span> baz");
		}

		[Test]
		public void Highlight_WhenMultiTerms_ShouldHighlightAllTerms()
		{
			"foo bar baz".Highlight(new[] { "bar", "baz" }).Should().Be("foo <span class=\"hl\">bar</span> <span class=\"hl\">baz</span>");
		}

		[Test]
		public void Highlight_WhenDifferingCase_ShouldHighlightTerms()
		{
			"Foo BAR baz".Highlight(new[] { "foo", "bar" }).Should().Be("<span class=\"hl\">Foo</span> <span class=\"hl\">BAR</span> baz");
		}

		[Test]
		public void Highlight_WhenNotCompleteWord_ShouldNotHighlightTerm()
		{
			"foo bar baz".Highlight(new[] { "fo" }).Should().Be("foo bar baz");
		}

		[Test]
		public void Highlight_WhenCompleteWordFollowedByPunctuation_ShouldHighlightTerm()
		{
			"foo bar! baz".Highlight(new[] { "bar" }).Should().Be("foo <span class=\"hl\">bar</span>! baz");
		}
	}
}
