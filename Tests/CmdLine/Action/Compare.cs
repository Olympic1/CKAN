using CKAN;
using NUnit.Framework;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class Compare
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

        private string lowVer = "v2.8.3";
        private string highVer = "v3.6.1";

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

        [Test]
        public void Compare_Parameters_Are_Null()
        {
            // Setup
            var args = "compare";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"{args} .* - argument.* missing, perhaps you forgot it"));
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }

        [Test]
        public void Compare_Version_Is_Higher()
        {
            // Setup
            var args = $"compare {highVer} {lowVer}";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"\"{highVer}\" is higher than \"{lowVer}\".", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Version_Is_Higher_MachineReadable()
        {
            // Setup
            var args = $"compare {highVer} {lowVer} --machine-readable";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("1", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Version_Is_Lower()
        {
            // Setup
            var args = $"compare {lowVer} {highVer}";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"\"{lowVer}\" is lower than \"{highVer}\".", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Version_Is_Lower_MachineReadable()
        {
            // Setup
            var args = $"compare {lowVer} {highVer} --machine-readable";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("-1", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Versions_Are_Identical()
        {
            // Setup
            var args = $"compare {lowVer} {lowVer}";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"\"{lowVer}\" and \"{lowVer}\" are the same versions.", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compare_Versions_Are_Identical_MachineReadable()
        {
            // Setup
            var args = $"compare {lowVer} {lowVer} --machine-readable";

            // Execute
            _console.ParseArguments(args);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("0", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }
    }
}