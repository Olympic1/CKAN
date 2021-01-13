using System.IO;
using System.Linq;
using CKAN;
using CKAN.Configuration;
using NUnit.Framework;
using Tests.Core.Configuration;
using Tests.Data;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class AuthToken
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

        private DisposableKSP _ksp;
        private string _tmpFile;
        private JsonConfiguration _reg;
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
            _ksp = new DisposableKSP();
            _tmpFile = Path.GetTempFileName();
            File.WriteAllText(_tmpFile, TestData.CommandLineConfig());
            _reg = new JsonConfiguration(_tmpFile);
            _config = new FakeConfiguration(_ksp.KSP, _ksp.KSP.Name);
            _manager = new GameInstanceManager(new NullUser(), _config)
            {
                CurrentInstance = _ksp.KSP,
                Configuration = _reg
            };
        }

        [OneTimeTearDown]
        public void TearDownOnce()
        {
            _manager.Dispose();
            _config.Dispose();
            File.Delete(_tmpFile);
            _ksp.Dispose();
        }

        [TestCase("authtoken", null, false)]
        [TestCase("authtoken", null)]
        [TestCase("authtoken", "add")]
        [TestCase("authtoken", "list")]
        [TestCase("authtoken", "remove")]
        public void AuthToken_Shows_HelpText(string arg, string sub, bool help = true)
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

            var args = help ? $"{arg} --help" : arg;

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("USAGE:", _output[2]);
            Assert.That(_output[3], Does.Match($@"ckan {text} \[options\]"));

            if (!help)
            {
                Assert.AreEqual("ERROR(S):", _output[4]);
                Assert.AreEqual("No verb selected.", _output[5]);
            }

            if (string.IsNullOrWhiteSpace(sub))
            {
                Assert.IsTrue(_output.Any(x => x.Contains("add")));
                Assert.IsTrue(_output.Any(x => x.Contains("list")));
                Assert.IsTrue(_output.Any(x => x.Contains("remove")));
            }

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("authtoken", "add")]
        [TestCase("authtoken", "remove")]
        public void AuthToken_Parameters_Are_Null(string arg, string sub)
        {
            // Setup
            var args = $"{arg} {sub}";

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"{sub} .* - argument.* missing, perhaps you forgot it"));
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }

        [Test]
        public void AuthToken_Add_Success()
        {
            // Execute
            _console.ParseArguments("authtoken add host3 token3", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.IsTrue(_reg.TryGetAuthToken("host3", out var token));
            Assert.AreEqual("token3", token);

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void AuthToken_List_Success()
        {
            // Execute
            _console.ParseArguments("authtoken list", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[2], Does.Match(@"host1 .* token1"));
            Assert.That(_output[3], Does.Match(@"host2 .* token2"));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void AuthToken_Remove_Invalid_Host()
        {
            // Execute
            _console.ParseArguments("authtoken remove host4", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("There is no host with the name \"host4\".", _output[0]);
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }

        [Test]
        public void AuthToken_Remove_Success()
        {
            // Execute
            _console.ParseArguments("authtoken remove host1", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.IsFalse(_reg.TryGetAuthToken("host1", out _));
            Assert.IsTrue(_reg.TryGetAuthToken("host2", out _));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }
    }
}