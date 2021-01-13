using System.Linq;
using CKAN;
using NUnit.Framework;

namespace Tests.CmdLine
{
    [TestFixture]
    public class HelpScreens
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

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

        [TestCase("available")]
        [TestCase("consoleui")]
        [TestCase("gui")]
        [TestCase("import")]
        [TestCase("install")]
        [TestCase("list")]
        [TestCase("remove")]
        [TestCase("replace")]
        [TestCase("scan")]
        [TestCase("search")]
        [TestCase("show")]
        [TestCase("update")]
        [TestCase("upgrade")]
        public void Verbs_With_Usage_And_Instance_Options(string arg)
        {
            // Setup
            var args = $"{arg} --help";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {arg} \[options\]"));

            Assert.IsTrue(_output.Any(x => x.Contains("--ksp")));
            Assert.IsTrue(_output.Any(x => x.Contains("--kspdir")));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("compare")]
        [TestCase("prompt")]
        public void Verbs_With_Usage_And_Common_Options(string arg)
        {
            // Setup
            var args = $"{arg} --help";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {arg} \[options\]"));

            Assert.IsFalse(_output.Any(x => x.Contains("--ksp")));
            Assert.IsFalse(_output.Any(x => x.Contains("--kspdir")));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Help_Flag_HelpText()
        {
            // Setup
            var args = "--help";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.IsTrue(_output.Any(x => x.Contains("consoleui")));
            Assert.IsTrue(_output.Any(x => x.Contains("gui")));
            Assert.IsTrue(_output.Any(x => x.Contains("scan")));
            Assert.IsTrue(_output.Any(x => x.Contains("authtoken")));
            Assert.IsTrue(_output.Any(x => x.Contains("available")));
            Assert.IsTrue(_output.Any(x => x.Contains("cache")));
            Assert.IsTrue(_output.Any(x => x.Contains("compare")));
            Assert.IsTrue(_output.Any(x => x.Contains("compat")));
            Assert.IsTrue(_output.Any(x => x.Contains("import")));
            Assert.IsTrue(_output.Any(x => x.Contains("install")));
            Assert.IsTrue(_output.Any(x => x.Contains("ksp")));
            Assert.IsTrue(_output.Any(x => x.Contains("list")));
            Assert.IsTrue(_output.Any(x => x.Contains("mark")));
            Assert.IsTrue(_output.Any(x => x.Contains("prompt")));
            Assert.IsTrue(_output.Any(x => x.Contains("remove")));
            Assert.IsTrue(_output.Any(x => x.Contains("repair")));
            Assert.IsTrue(_output.Any(x => x.Contains("replace")));
            Assert.IsTrue(_output.Any(x => x.Contains("repo")));
            Assert.IsTrue(_output.Any(x => x.Contains("search")));
            Assert.IsTrue(_output.Any(x => x.Contains("show")));
            Assert.IsTrue(_output.Any(x => x.Contains("update")));
            Assert.IsTrue(_output.Any(x => x.Contains("upgrade")));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Version_Flag_HelpText()
        {
            // Setup
            var args = "--version";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual(Meta.GetVersion(VersionFormat.Full), _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }
    }
}