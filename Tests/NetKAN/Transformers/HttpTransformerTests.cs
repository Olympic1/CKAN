using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using CKAN.NetKAN.Model;
using CKAN.NetKAN.Transformers;

namespace Tests.NetKAN.Transformers
{
    [TestFixture]
    public sealed class HttpTransformerTests
    {
        private readonly TransformOptions opts = new TransformOptions(1, null, null, null, false, null);

        [TestCase("#/ckan/github/foo/bar")]
        [TestCase("#/ckan/netkan/http://awesomemod.example/awesomemod.netkan")]
        [TestCase("#/ckan/spacedock/1")]
        [TestCase("#/ckan/curse/1")]
        [TestCase("#/ckan/foo")]
        public void DoesNotAlterMetadataWhenNonMatching(string kref)
        {
            // Arrange
            var sut = new HttpTransformer();
            var json = new JObject();
            json["spec_version"] = 1;
            json["$kref"] = kref;

            // Act
            var result = sut.Transform(new Metadata(json), opts).First();
            var transformedJson = result.Json();

            // Assert
            Assert.That(transformedJson, Is.EqualTo(json),
                "HttpTransformed should not alter the metatadata when it does not match the $kref."
            );
        }
    }
}
