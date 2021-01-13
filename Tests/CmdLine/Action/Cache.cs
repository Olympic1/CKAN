using System;
using System.IO;
using System.Linq;
using Autofac;
using CKAN;
using CKAN.Configuration;
using NUnit.Framework;
using Tests.Core.Configuration;
using Tests.Data;

namespace Tests.CmdLine.Action
{
    [TestFixture]
    public class Cache
    {
        private int _exitCode;
        private string[] _output;
        private ConsoleOutput _console;

        private DisposableKSP _ksp;
        private string _tmpFile;
        private JsonConfiguration _reg;
        private FakeConfiguration _config;
        private GameInstanceManager _manager;

        private string _cacheDir;
        private NetFileCache _cache;

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

            _cacheDir = _manager.CurrentInstance.DownloadCacheDir();
            Directory.CreateDirectory(_cacheDir);
            _cache = new NetFileCache(_manager, _cacheDir);
        }

        [OneTimeTearDown]
        public void TearDownOnce()
        {
            _cache.Dispose();
            Directory.Delete(_cacheDir);

            _manager.Dispose();
            _config.Dispose();
            File.Delete(_tmpFile);
            _ksp.Dispose();
        }

        [TestCase("cache", null, false)]
        [TestCase("cache", null)]
        [TestCase("cache", "clear")]
        [TestCase("cache", "list")]
        [TestCase("cache", "reset")]
        [TestCase("cache", "set")]
        [TestCase("cache", "setlimit")]
        [TestCase("cache", "showlimit")]
        public void Cache_Shows_HelpText(string arg, string sub, bool help = true)
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
            Assert.AreEqual("USAGE:", _console.Output[2]);
            Assert.That(_console.Output[3], Does.Match($@"ckan {text} \[options\]"));

            if (!help)
            {
                Assert.AreEqual("ERROR(S):", _output[4]);
                Assert.AreEqual("No verb selected.", _output[5]);
            }

            if (string.IsNullOrWhiteSpace(sub))
            {
                Assert.IsTrue(_output.Any(x => x.Contains("clear")));
                Assert.IsTrue(_output.Any(x => x.Contains("list")));
                Assert.IsTrue(_output.Any(x => x.Contains("reset")));
                Assert.IsTrue(_output.Any(x => x.Contains("set")));
                Assert.IsTrue(_output.Any(x => x.Contains("setlimit")));
                Assert.IsTrue(_output.Any(x => x.Contains("showlimit")));
            }

            Assert.AreEqual(Exit.Ok, _console.ExitCode);
        }

        [TestCase("cache", "set")]
        public void Cache_Parameters_Are_Null(string arg, string sub)
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

        [TestCase("10")]
        [TestCase(null)]
        public void Cache_SetLimit_Success(string size)
        {
            // Setup
            var args = string.IsNullOrWhiteSpace(size) ? "cache setlimit" : $"cache setlimit {size}";

            // Execute
            _console.ParseArguments(args, _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            var cfg = ServiceLocator.Container.Resolve<IConfiguration>();

            // Verify
            if (int.TryParse(size, out var intSize))
            {
                var bytes = intSize * 1024 * 1024;
                Assert.AreEqual(bytes, cfg.CacheSizeLimit);
            }
            else
            {
                Assert.AreEqual(null, cfg.CacheSizeLimit);
            }

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [TestCase("2")]
        [TestCase("Unlimited")]
        public void Cache_ShowLimit_Success(string size)
        {
            // Setup
            _manager.Configuration.CacheSizeLimit = size.Equals("Unlimited") ? (long?)null : int.Parse(size);

            // Execute
            _console.ParseArguments("cache showlimit", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            size += size.Equals("Unlimited") ? "" : " B";

            // Verify
            Assert.AreEqual($"Cache limit set to {size}.", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Cache_Clear_Success()
        {
            // Setup
            var url = new Uri("http://example.com/");
            var file = TestData.DogeCoinFlagZip();

            Assert.IsFalse(_cache.IsCached(url));
            _cache.Store(url, file);
            Assert.IsTrue(_cache.IsCached(url));

            // Execute
            _console.ParseArguments("cache clear", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("Cleared download cache.", _output[0]);
            Assert.IsFalse(_cache.IsCached(url));

            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Cache_List_Success()
        {
            // Execute
            _console.ParseArguments("cache list", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual("Download cache is set to \"dci\".", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Cache_Reset_Success()
        {
            // Setup
            var def = JsonConfiguration.DefaultDownloadCacheDir;

            // Execute
            _console.ParseArguments("cache reset", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"Download cache reset to \"{def}\".", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }

        [Test]
        public void Cache_Set_Invalid_Path()
        {
            // Setup
            var temp = "this/path/should/not/exists";

            // Execute
            _console.ParseArguments($"cache set {temp}", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.That(_output[0], Does.Match($@"Invalid path: {temp}"));
            Assert.AreEqual(Exit.Error, _exitCode);
        }

        [Test]
        public void Cache_Set_Success()
        {
            // Execute
            _console.ParseArguments($"cache set {_cacheDir}", _manager);
            _exitCode = _console.ExitCode;
            _output = _console.Output;

            // Verify
            Assert.AreEqual($"Download cache set to \"{_cacheDir}\".", _output[0]);
            Assert.AreEqual(Exit.Ok, _exitCode);
        }
    }
}