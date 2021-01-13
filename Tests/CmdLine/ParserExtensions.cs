using System;
using CKAN.CmdLine;
using CommandLine;
using NUnit.Framework;

namespace Tests.CmdLine
{
    [TestFixture]
    public class ParserExtensions
    {
        [Test]
        public void ParseVerbs_Bad_Arguments_Throws_Null()
        {
            var parser = new Parser();
            string[] args = null;

            Type[] types = { typeof(FakeVerb) };

            Assert.Throws<ArgumentNullException>(() => parser.ParseVerbs(args, types));

            parser.Dispose();
        }

        [Test]
        public void ParseVerbs_Bad_Parser_Throws_Null()
        {
            Parser parser = null;
            var args = "argument".Split();

            Type[] types = { typeof(FakeVerb) };

            Assert.Throws<ArgumentNullException>(() => parser.ParseVerbs(args, types));
        }

        [Test]
        public void ParseVerbs_Bad_Types_Throws_Null()
        {
            var parser = new Parser();
            var args = "argument".Split();

            Type[] types = null;

            Assert.Throws<ArgumentNullException>(() => parser.ParseVerbs(args, types));

            parser.Dispose();
        }

        [Test]
        public void ParseVerbs_Bad_Types_Throws_OutOfRange()
        {
            var parser = new Parser();
            var args = "argument".Split();

            Type[] types = { };

            Assert.Throws<ArgumentOutOfRangeException>(() => parser.ParseVerbs(args, types));

            parser.Dispose();
        }
    }

    [Verb("fake")]
    internal class FakeVerb
    {
        [Option('x')]
        public string FakeOption { get; set; }
    }
}