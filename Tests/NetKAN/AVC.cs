using System.IO;
using CKAN.NetKAN.Sources.Avc;
using CKAN.Versioning;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests.Data;

namespace Tests.NetKAN
{
    [TestFixture]
    public class AVC
    {
        [Test]
        public void Json()
        {
            string json = TestData.KspAvcJson();

            var avc = JsonConvert.DeserializeObject<AvcVersion>(json);

            Assert.AreEqual("0.24.2", avc?.ksp_version?.ToString());
            Assert.AreEqual("0.24.0", avc?.ksp_version_min?.ToString());
            Assert.AreEqual("0.24.2", avc?.ksp_version_max?.ToString());
        }

        [Test]
        public void JsonOneLineVersion()
        {
            string json = TestData.KspAvcJsonOneLineVersion();

            var avc = JsonConvert.DeserializeObject<AvcVersion>(json);

            Assert.AreEqual("0.24.2", avc?.ksp_version?.ToString());
            Assert.AreEqual("0.24.0", avc?.ksp_version_min?.ToString());
            Assert.AreEqual("1.0.0",  avc?.ksp_version_max?.ToString());
        }

        [Test]
        public void WildcardMajor_OutputsAnyVersion()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{""MAJOR"":-1, ""MINOR"":-1, ""PATCH"":-1}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(!result.IsMajorDefined);
        }

        [Test]
        public void WildcardMinor_VersionOnlyHasMajor()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{""MAJOR"":1, ""MINOR"":-1, ""PATCH"":-1}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(result, Is.EqualTo(GameVersion.Parse("1")));
        }

        [Test]
        public void WildcardPatch_VersionOnlyHasMajorMinor()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{""MAJOR"":1, ""MINOR"":5, ""PATCH"":-1}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(result, Is.EqualTo(GameVersion.Parse("1.5")));
        }

        [Test]
        public void MissingMajor_OutputsAnyVersion()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(!result.IsMajorDefined);
        }

        [Test]
        public void MissingMinor_VersionOnlyHasMajor()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{""MAJOR"":1}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(result, Is.EqualTo(GameVersion.Parse("1")));
        }

        [Test]
        public void MissingPatch_VersionOnlyHasMajorMinor()
        {
            var converter = new JsonAvcToGameVersion();
            string json = @"{""MAJOR"":1, ""MINOR"":5}";
            var reader = new JsonTextReader(new StringReader(json));
            var result = (GameVersion)converter.ReadJson(reader, null, null, null)!;
            Assert.That(result, Is.EqualTo(GameVersion.Parse("1.5")));
        }

    }
}
