using CKAN;
using NUnit.Framework;

namespace Tests.CmdLine
{
    [TestFixture]
    public class Options
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

        [TestCase("import")]
        [TestCase("install")]
        [TestCase("mark", "auto")]
        [TestCase("mark", "user")]
        [TestCase("remove")]
        [TestCase("replace")]
        [TestCase("repo", "add")]
        [TestCase("repo", "default")]
        [TestCase("repo", "forget")]
        [TestCase("show")]
        [TestCase("upgrade")]
        public void Options_Parameters_Are_Null(string arg, string sub = null)
        {
            // Setup
            if (!string.IsNullOrWhiteSpace(sub))
                arg += $" {sub}";
            else
                sub = arg;

            // Execute
            _console.ParseArguments(arg);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"{sub} .* - argument.* missing, perhaps you forgot it"));
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }
    }
}