using System.Collections.Generic;

using log4net;

using CKAN.NetKAN.Model;
using CKAN.NetKAN.Extensions;

namespace CKAN.NetKAN.Transformers
{
    /// <summary>
    /// An <see cref="ITransformer"/> that sets the spec version to match the metadata.
    /// </summary>
    internal sealed class SpecVersionTransformer : ITransformer
    {
        /// <summary>
        /// Defines the name of this transformer
        /// </summary>
        public string Name => "spec_version";

        public IEnumerable<Metadata> Transform(Metadata          metadata,
                                               TransformOptions opts)
        {
            var minVersion = SpecVersionAnalyzer.MinimumSpecVersion(metadata.AllJson);
            if (metadata.SpecVersion == null || metadata.SpecVersion != minVersion)
            {
                var json = metadata.Json();
                log.InfoFormat("Setting spec version {0}", minVersion);
                json["spec_version"] = minVersion.ToSpecVersionJson();
                yield return new Metadata(json);
                yield break;
            }
            yield return metadata;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(SpecVersionTransformer));
    }
}
