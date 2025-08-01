using System;
using System.Linq;

using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Moq;

using CKAN.NetKAN.Model;
using CKAN.NetKAN.Services;
using CKAN.NetKAN.Sources.Spacedock;
using CKAN.NetKAN.Sources.Github;
using CKAN.NetKAN.Transformers;

namespace Tests.NetKAN.Transformers
{
    [TestFixture]
    public sealed class SpacedockTransformerTests
    {

        // GH #199: Don't pre-fill KSP version fields if we see a ksp_min/max
        [Test]
        public void DoesNotReplaceGameVersionProperties()
        {
            // Arrange
            var http = new Mock<IHttpService>();
            http.Setup(h => h.DownloadText(It.Is<Uri>(u => u.AbsolutePath.Contains("/api/mod/")),
                                           It.IsAny<string?>(), It.IsAny<string?>()))
                .Returns((Uri u, string? _, string? _) => $@"{{
                                        ""id"":                 1,
                                        ""name"":               ""Dogecoin Flag"",
                                        ""short_description"":  ""Such test. Very unit. Wow."",
                                        ""license"":            ""CC-BY"",
                                        ""author"":             ""pjf"",
                                        ""default_version_id"": 1,
                                        ""versions"": [
                                            {{
                                                ""id"":               1,
                                                ""friendly_version"": ""0.25"",
                                                ""game_version"":     ""1.12.5"",
                                                ""download_path"":    ""http://example.com/"",
                                            }}
                                        ],
                                    }}");

            var mGhApi = new Mock<IGithubApi>();
            mGhApi.Setup(i => i.GetRepo(It.IsAny<GithubRef>()))
                .Returns(new GithubRepo
                {
                    HtmlUrl = "https://github.com/ExampleAccount/ExampleProject"
                });

            var sut      = new SpacedockTransformer(new SpacedockApi(http.Object), mGhApi.Object);
            var opts     = new TransformOptions(1, null, null, null, false, null);
            var metadata = new Metadata(new JObject()
            {
                { "spec_version",    1                    },
                { "$kref",           "#/ckan/spacedock/1" },
                { "ksp_version_min", "0.23.5"             },
            });

            // Act
            var result          = sut.Transform(metadata, opts).First();
            var transformedJson = result.Json();

            // Assert
            Assert.AreEqual(null,     (string?)transformedJson["ksp_version"]);
            Assert.AreEqual(null,     (string?)transformedJson["ksp_version_max"]);
            Assert.AreEqual("0.23.5", (string?)transformedJson["ksp_version_min"]);
        }

    }
}
