using CKAN;
using NUnit.Framework;

namespace Tests.CmdLine
{
    [TestFixture]
    public class UsageTexts
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

        [TestCase("mark", null)]
        [TestCase("mark", null, false)]
        [TestCase("mark", "auto")]
        [TestCase("mark", "user")]
        public void Mark_With_Usage_Text(string arg, string sub, bool help = true)
        {
            var text = Execute(arg, sub, help);

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {text} \[options\]"));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("repair", null)]
        [TestCase("repair", null, false)]
        [TestCase("repair", "registry")]
        public void Repair_With_Usage_Text(string arg, string sub, bool help = true)
        {
            var text = Execute(arg, sub, help);

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {text} \[options\]"));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("repo", null)]
        [TestCase("repo", null, false)]
        [TestCase("repo", "add")]
        [TestCase("repo", "available")]
        [TestCase("repo", "default")]
        [TestCase("repo", "forget")]
        [TestCase("repo", "list")]
        public void Repo_With_Usage_Text(string arg, string sub, bool help = true)
        {
            var text = Execute(arg, sub, help);

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {text} \[options\]"));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        private string Execute(string arg, string sub, bool help)
        {
            // Setup
            string text;
            if (!string.IsNullOrWhiteSpace(sub))
            {
                arg += $" {sub}";
                text = arg;
            }
            else
            {
                text = $"{arg} <command>";
            }

            var input = help ? $"{arg} --help" : arg;

            // Execute
            _console.ParseArguments(input);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            return text;
        }
    }
}