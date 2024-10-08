using System;
using System.IO;
using System.Net;
using System.Text;

using log4net;
using Newtonsoft.Json;

using CKAN.NetKAN.Services;

namespace CKAN.NetKAN.Sources.Curse
{
    internal sealed class CurseApi : ICurseApi
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CurseApi));

        private const string CurseApiBaseOld = "https://api.cfwidget.com/project/";
        private const string CurseApiBase    = "https://api.cfwidget.com/kerbal/ksp-mods/";

        private readonly IHttpService _http;
        private readonly string?      _userAgent;

        public CurseApi(IHttpService http, string? userAgent)
        {
            _http = http;
            _userAgent = userAgent;
        }

        public CurseMod? GetMod(string nameOrId)
        {
            string? json = null;
            try
            {
                json = Call(nameOrId);
            }
            catch (WebException e)
            {
                // CurseForge returns a valid json with an error message in some cases.
                if (e.Response != null)
                {
                    json = new StreamReader(e.Response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                }
            }
            if (json == null)
            {
                throw new Kraken("Got nothing from Curse!");
            }
            // Check if the mod has been removed from Curse and if it corresponds to a KSP mod.
            var error = JsonConvert.DeserializeObject<CurseError>(json);
            if (error != null && !string.IsNullOrWhiteSpace(error.error))
            {
                throw new Kraken($"Could not get the mod from Curse, reason: {error.message}.");
            }
            return CurseMod.FromJson(json, _userAgent);
        }

        private string? Call(string nameOrId)
        {
            // If it's numeric, use the old URL format,
            // otherwise use the new.
            var url = int.TryParse(nameOrId, out int id)
                ? CurseApiBaseOld + id
                : CurseApiBase + nameOrId;
            Log.InfoFormat("Calling {0}", url);
            return _http.DownloadText(new Uri(url));
        }
    }
}
