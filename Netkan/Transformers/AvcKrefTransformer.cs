using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;

using CKAN.NetKAN.Extensions;
using CKAN.NetKAN.Model;
using CKAN.NetKAN.Services;
using CKAN.Avc;
using CKAN.NetKAN.Sources.Github;

namespace CKAN.NetKAN.Transformers
{
    /// <summary>
    /// An <see cref="ITransformer"/> that looks up data from a KSP-AVC URL.
    /// </summary>
    internal sealed class AvcKrefTransformer : ITransformer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AvcKrefTransformer));

        public string Name => "avc-kref";
        private readonly IHttpService httpSvc;
        private readonly IGithubApi?  githubSrc;

        public AvcKrefTransformer(IHttpService http, IGithubApi? github)
        {
            httpSvc   = http;
            githubSrc = github;
        }

        public IEnumerable<Metadata> Transform(Metadata metadata, TransformOptions opts)
        {
            if (metadata.Kref?.Source == "ksp-avc" && metadata.Kref.Id != null)
            {
                Log.InfoFormat("Executing KSP-AVC $kref transformation with {0}", metadata.Kref);

                var url = new Uri(metadata.Kref.Id);
                if ((githubSrc?.DownloadText(url)
                             ?? httpSvc.DownloadText(Net.GetRawUri(url)))
                         is string contents
                    && JsonConvert.DeserializeObject<AvcVersion>(contents)
                        is AvcVersion remoteAvc)
                {
                    var json = metadata.Json();
                    Log.DebugFormat("Input metadata:{0}{1}", Environment.NewLine, metadata.AllJson);
                    json.SafeAdd("name",     remoteAvc.Name);
                    json.Remove("$kref");
                    json.SafeAdd("download", remoteAvc.Download);

                    // Set .resources.repository based on GITHUB properties
                    if (remoteAvc.Github?.Username != null && remoteAvc.Github?.Repository != null)
                    {
                        // Make sure resources exist.
                        if (json["resources"] == null)
                        {
                            json["resources"] = new JObject();
                        }

                        var resourcesJson = (JObject?)json["resources"];
                        resourcesJson?.SafeAdd("repository", $"https://github.com/{remoteAvc.Github.Username}/{remoteAvc.Github.Repository}");
                    }
                    // Use standard KSP-AVC logic to set version and the ksp_version_* properties
                    AvcTransformer.ApplyVersions(metadata, json, remoteAvc);
                    Log.DebugFormat("Transformed metadata:{0}{1}", Environment.NewLine, json);

                    yield return new Metadata(json);
                    yield break;
                }
            }
            yield return metadata;
        }
    }
}
