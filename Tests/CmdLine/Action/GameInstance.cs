using System.IO;
using System.Linq;
using CKAN;
using NUnit.Framework;
using Tests.Core.Configuration;
using Tests.Data;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class GameInstance
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

        [TestCase("ksp", null)]
        [TestCase("ksp", null, false)]
        [TestCase("ksp", "add")]
        [TestCase("ksp", "clone")]
        [TestCase("ksp", "default")]
        [TestCase("ksp", "fake")]
        [TestCase("ksp", "forget")]
        [TestCase("ksp", "list")]
        [TestCase("ksp", "rename")]
        public void Ksp_Shows_HelpText(string arg, string sub, bool help = true)
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
                Assert.IsTrue(_output.Any(x => x.Contains("clone")));
                Assert.IsTrue(_output.Any(x => x.Contains("default")));
                Assert.IsTrue(_output.Any(x => x.Contains("fake")));
                Assert.IsTrue(_output.Any(x => x.Contains("forget")));
                Assert.IsTrue(_output.Any(x => x.Contains("list")));
                Assert.IsTrue(_output.Any(x => x.Contains("rename")));
            }

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("ksp", "add")]
        [TestCase("ksp", "clone")]
        [TestCase("ksp", "fake")]
        [TestCase("ksp", "forget")]
        [TestCase("ksp", "rename")]
        public void Ksp_Parameters_Are_Null(string arg, string sub)
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

        #region Add

        [Test]
        public void Ksp_Add_Name_Exists()
        {
            // Setup
            var name = _config.GetInstance(0).Item1;

            // Execute
            _console.ParseArguments($"ksp add {name} path", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"Install with the name \"{name}\" already exists, aborting...", _output[0]);
            Assert.AreEqual(Exit.BadOpt, _exitCode);
        }

        [Test]
        public void Ksp_Add_NotKSPDir()
        {
            // Setup
            var path = "invalid/path";

            // Execute
            _console.ParseArguments($"ksp add test2 {path}", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($"Sorry, \"{path}\" does not appear to be a KSP directory"));
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        [Test]
        public void Ksp_Add_Success()
        {
            using (var ksp = new DisposableKSP())
            {
                // Setup
                ksp.KSP.Name = "test2";

                // Execute
                _console.ParseArguments($"ksp add {ksp.KSP.Name} {ksp.KSP.GameDir()}", _manager);
                _exitCode = _console.ExitCode;
                _output = _console.Output;

                // Verify
                Assert.AreEqual($"Added \"{ksp.KSP.Name}\" with root \"{ksp.KSP.GameDir()}\" to known installs.", _output[0]);
                Assert.AreEqual(Exit.Ok, _exitCode);
            }
        }

        #endregion

        #region Clone

        public void Ksp_Clone_NotKSPDir()
        {
            // KSP dir has no GameData
            // Setup
            string test = _manager.Instances["test2"].game.PrimaryModDirectory(_manager.Instances["test2"]);
            Directory.Delete(test, true);

            // Execute
            _console.ParseArguments($"ksp clone {_ksp.KSP.GameDir()} name path", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"", _output[0]);
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        public void Ksp_Clone_Not_Empty()
        {
            // New path is not empty
        }

        // TODO: IOException??

        public void Ksp_Clone_Not_found()
        {
            // Instance doesn't exist
        }

        public void Ksp_Clone_Name_Exists()
        {
            // Name already exists
        }

        [TestCase("badName")]
        [TestCase("path/does/not/exist")]
        public void Ksp_Clone_Instance_Not_Exists(string namePath)
        {
            // Execute
            _console.ParseArguments($"ksp clone {namePath} name path", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"No instance found with this name or at this path: \"{namePath}\".", _output[0]);
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        public void Ksp_Clone_Success()
        {

        }

        #endregion

        #region Default

        public void Ksp_Default_Not_Found()
        {
            // Instance doesn't exist
        }

        public void Ksp_Default_NotKSPDir()
        {
            // KSP dir has no GameData
        }

        public void Ksp_Default_Success()
        {

        }

        #endregion

        #region Fake

        public void Ksp_Fake_Invalid_MH_Version()
        {
            // MH version is not valid (1.8.1A)
        }

        public void Ksp_Fake_Invalid_BG_Version()
        {
            // BG version is not valid (1.5.1A)
        }

        public void Ksp_Fake_Invalid_KSP_Version()
        {
            // KSP version is not valid (1.8.1A)
        }

        public void Ksp_Fake_Name_Exists()
        {
            // Name already exists
        }

        public void Ksp_Fake_Not_Empty()
        {

        }

        public void Ksp_Fake_Old_Version()
        {

        }

        public void Ksp_Fake_NotKSPDir()
        {
            // KSP dir has no GameData
        }

        public void Ksp_Fake_Instance_Exists()
        {
            // New path is not empty
        }

        public void Ksp_Fake_Success()
        {

        }

        #endregion

        #region Forget

        public void Ksp_Forget_Not_Found()
        {
            // Instance doesn't exist
        }

        public void Ksp_Forget_Success()
        {

        }

        #endregion

        #region List

        public void Ksp_List_Success()
        {

        }

        #endregion

        #region Rename

        public void Ksp_Rename_Not_Found()
        {
            // Instance doesn't exist
        }

        public void Ksp_Rename_Success()
        {

        }

        #endregion
    }
}
