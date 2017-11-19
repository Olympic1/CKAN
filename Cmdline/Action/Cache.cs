using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using CommandLine;
using CKAN;
using log4net;

namespace CKAN.CmdLine
{
    public class Cache : ISubCommand
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Cache));

        public CKAN.KSP CurrentInstance { get; set; }
        public IUser User { get; set; }
        public string option;
        public object suboptions;

        public Cache(CKAN.KSP current_instance,IUser user)
        {
            CurrentInstance = current_instance;
            User = user;
        }

        internal class CacheSubOptions : CommonOptions
        {
            [VerbOption("list", HelpText="List the Download Cache Directory location")]
            public CommonOptions ListOptions { get; set; }

            [VerbOption("set", HelpText="Set the Download Cache Directory location")]
            public CommonOptions SetOptions { get; set; }
        }

        internal class ListOptions : CommonOptions
        {
        }

        internal class SetOptions : CommonOptions
        {
            [ValueOption(0)]
            public string path { get; set; }
        }


        internal void Parse(string option, object suboptions)
        {
            this.option = option;
            this.suboptions = suboptions;
        }

        // This is required by ISubCommand
        public int RunSubCommand(SubCommandOptions unparsed)
        {
            string[] args = unparsed.options.ToArray();

            if (args.Length == 0)
            {
                // There's got to be a better way of showing help...
                args = new string[1];
                args[0] = "help";
            }

            // Parse and process our sub-verbs
            Parser.Default.ParseArgumentsStrict(args, new CacheSubOptions (), Parse);

            // That line above will have set our 'option' and 'suboption' fields.

            switch (option)
            {
                case "list":
                    return ListCacheDirectory((ListOptions)suboptions);

                case "set":
                    return SetCacheDirectory((SetOptions)suboptions);

                default:
                    User.RaiseMessage("Unknown command: cache {0}", option);
                    return Exit.BADOPT;
            }
        }

        private int ListCacheDirectory(ListOptions options)
        {
            User.RaiseMessage("Location for Cached mod downloads:");
            var registry = RegistryManager.Instance(CurrentInstance).registry;

            //User.RaiseMessage(registry.DownloadCacheDir);

            return Exit.OK;
        }

        private int SetCacheDirectory(SetOptions options)
        {
            if (options.path == null)
            {
                User.RaiseError("set <path> - argument missing, perhaps you forgot it?");
                return Exit.BADOPT;
            }

            var registry = RegistryManager.Instance(CurrentInstance).registry;
            log.DebugFormat("About to set Download Cache Directory to '{0}'", options.path);

            //registry.DownloadCacheDir = KSPPathUtils.NormalizePath(options.path);


            return Exit.OK;
        }

        
    }
}

