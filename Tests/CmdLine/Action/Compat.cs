using System.Collections.Generic;
using System.Linq;
using CKAN;
using CKAN.Versioning;
using NUnit.Framework;
using Tests.Core.Configuration;
using Tests.Data;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class Compat
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

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
            _ksp = new DisposableKSP();
            _config = new FakeConfiguration(_ksp.KSP, _ksp.KSP.Name);
            _manager = new GameInstanceManager(new NullUser(), _config)
            {
                CurrentInstance = _ksp.KSP
            };
            _manager.CurrentInstance.SetCompatibleVersions(new List<GameVersion>
            {
                new GameVersion(1, 4, 4),
                new GameVersion(1, 4, 3)
            });
        }

        [OneTimeTearDown]
        public void TearDownOnce()
        {
            _manager.Dispose();
            _config.Dispose();
            _ksp.Dispose();
        }

        [TestCase("compat", null, false)]
        [TestCase("compat", null)]
        [TestCase("compat", "add")]
        [TestCase("compat", "forget")]
        [TestCase("compat", "list")]
        public void Compat_Shows_HelpText(string arg, string sub, bool help = true)
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
            _console.ParseArguments(input, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("USAGE:", _console.Output[2]);
            Assert.That(_console.Output[3], Does.Match($@"ckan {text} \[options\]"));

            if (!help)
            {
                Assert.AreEqual("ERROR(S):", _output[4]);
                Assert.AreEqual("No verb selected.", _output[5]);
            }

            if (string.IsNullOrWhiteSpace(sub))
            {
                Assert.IsTrue(_output.Any(x => x.Contains("add")));
                Assert.IsTrue(_output.Any(x => x.Contains("forget")));
                Assert.IsTrue(_output.Any(x => x.Contains("list")));
            }

            Assert.AreEqual(Exit.Ok, _console.ExitCode);
        }

        [TestCase("compat", "add")]
        [TestCase("compat", "forget")]
        public void Compat_Parameters_Are_Null(string arg, string sub)
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
        public void Compat_Add_Invalid_Version()
        {
            // Execute
            _console.ParseArguments("compat add null", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("Invalid KSP version.", _output[0]);
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        [Test]
        public void Compat_Add_Success()
        {
            // Execute
            _console.ParseArguments("compat add 1.4.5", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.IsTrue(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 5)));
            Assert.IsTrue(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 4)));
            Assert.IsTrue(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 3)));
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compat_Forget_Invalid_Version()
        {
            // Execute
            _console.ParseArguments("compat forget null", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("Invalid KSP version.", _output[0]);
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        [Test]
        public void Compat_Forget_Not_Actual_Version()
        {
            // Execute
            _console.ParseArguments("compat forget 0.25.0.642", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("Cannot forget actual KSP version.", _output[0]);
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        [Test]
        public void Compat_Forget_Success()
        {
            // Setup
            _manager.CurrentInstance.SetCompatibleVersions(new List<GameVersion>
            {
                new GameVersion(1, 4, 5),
                new GameVersion(1, 4, 4),
                new GameVersion(1, 4, 3)
            });

            // Execute
            _console.ParseArguments("compat forget 1.4.5", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.IsFalse(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 5)));
            Assert.IsTrue(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 4)));
            Assert.IsTrue(_manager.CurrentInstance.GetCompatibleVersions().Contains(new GameVersion(1, 4, 3)));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Compat_List_Success()
        {
            // Execute
            _console.ParseArguments("compat list", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[2], Does.Match("0.25.0.642"));
            Assert.That(_output[3], Does.Match("1.4.4"));
            Assert.That(_output[4], Does.Match("1.4.3"));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }
    }
}