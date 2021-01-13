using System.Linq;
using CKAN;
using NUnit.Framework;
using Tests.Core.Configuration;
using Tests.Data;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class Search
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

        private string _registry;
        private DisposableKSP _ksp;
        private FakeConfiguration _config;
        private GameInstanceManager _manager;

        [SetUp]
        public void Setup()
        {
            _exitCode = Exit.Ok;
            _output = null;
            _console = new ConsoleOutput();
        }

        [TearDown]
        public void TearDown()
        {
            _console.Dispose();
            _console = null;
        }

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            _registry = TestData.TestRegistry();
            _ksp = new DisposableKSP(null, _registry);
            _config = new FakeConfiguration(_ksp.KSP, null);
            _manager = new GameInstanceManager(new NullUser(), _config);
        }

        [OneTimeTearDown]
        public void TearDownOnce()
        {
            _manager.Dispose();
            _config.Dispose();
            _ksp.Dispose();
        }

        [TestCase("NoTerm", null)]
        [TestCase(null, "NoAuthor")]
        [TestCase("NoTerm", "NoAuthor")]
        public void Search_NoModsFound(string term, string author)
        {
            // Setup
            var args = "search";
            var result = "Couldn't find any mod";

            if (!string.IsNullOrWhiteSpace(term))
            {
                args += $" {term}";
                result += $" matching \"{term}\"";
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                args += $" --author {author}";
                result += $" by \"{author}\"";
            }

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"{result}.", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("AGExt", null, false)]
        [TestCase("AGExt", null, true)]
        [TestCase(null, "Diazo", false)]
        [TestCase(null, "Diazo", true)]
        [TestCase("AGExt", "Diazo", false)]
        [TestCase("AGExt", "Diazo", true)]
        public void Search_FoundMods(string term, string author, bool all)
        {
            // Setup
            var args = "search";
            var result = "Found . compatible";

            if (all)
            {
                result += " and . incompatible";
            }

            result += " mods";

            if (!string.IsNullOrWhiteSpace(term))
            {
                args += $" {term}";
                result += $" matching \"{term}\"";
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                args += $" --author {author}";
                result += $" by \"{author}\"";
            }

            if (all)
            {
                args += " --all";
            }

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"{result}"));
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("ModuleManager", false)]
        [TestCase("HangarExtender", true)]
        public void Search_Detailed(string term, bool all)
        {
            // Setup
            var args = $"search {term} --detail";
            args += all ? " --all" : "";

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            if (all)
            {
                Assert.That(_output[0], Does.Match($@"Found . compatible and . incompatible mods matching ""{term}"""));
                Assert.IsFalse(_output.Any(x => x.Contains("Matching compatible mods")));
                Assert.IsTrue(_output.Any(x => x.Contains("Matching incompatible mods")));
            }
            else
            {
                Assert.That(_output[0], Does.Match($@"Found . compatible mods matching ""{term}"""));
                Assert.IsTrue(_output.Any(x => x.Contains("Matching compatible mods")));
                Assert.IsFalse(_output.Any(x => x.Contains("Matching incompatible mods")));
            }

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Parameters_Are_Null()
        {
            // Setup
            var args = "search";

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"{args} .* - argument.* missing, perhaps you forgot it"));
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }
    }
}