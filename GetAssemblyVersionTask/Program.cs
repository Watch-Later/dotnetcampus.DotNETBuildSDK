﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using CommandLine;
using dotnetCampus.Configurations;
using dotnetCampus.DotNETBuild.Context;
using dotnetCampus.DotNETBuild.Utils;

namespace GetAssemblyVersionTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<AssmeblyOption>(args).WithParsed(option =>
            {
                var appConfigurator = AppConfigurator.GetAppConfigurator();
                var compileConfiguration = appConfigurator.Of<CompileConfiguration>();

#if DEBUG
                var fileSniff = new FileSniff(appConfigurator);
                fileSniff.Sniff();
#endif

                var file = option.AssemblyInfoFile;
                var codeDirectory = compileConfiguration.CodeDirectory;

                file = Path.Combine(codeDirectory, file);
                file = Path.GetFullPath(file);

                Log.Info($"Start read assmebly info file {file}");

                if (!File.Exists(file))
                {
                    throw new ArgumentException($"The assmebly info file {file} can not be found.");
                }

                var formatRegex = option.VersionFormatRegex;
                if (string.IsNullOrEmpty(formatRegex))
                {
                    formatRegex = "Version = \\\"(\\d+.\\d+.\\d+)\\\";";
                }

                Log.Info($"VersionFormatRegex: {formatRegex}");

                var content = File.ReadAllText(file);
                var match = Regex.Match(content,formatRegex);
                if (match.Success)
                {
                    var assemblyVersion = match.Groups[1].Value;

                    Log.Info($"assembly version: {assemblyVersion}");

                    var lastVersion = 0;
                    var gitConfiguration = appConfigurator.Of<GitConfiguration>();
                    if (gitConfiguration.GitCount != null)
                    {
                        Log.Info($"GitCount: {gitConfiguration.GitCount}");
                        lastVersion = gitConfiguration.GitCount.Value;
                    }

                    var appVersion = $"{assemblyVersion}.{lastVersion}";
                    Log.Info($"app version: {appVersion}");
                    compileConfiguration.AppVersion = appVersion;
                }
                else
                {
                    throw new ArgumentException($"Can not math VersionFormatRegex={formatRegex} in assmebly info file {file} \r\n The file content:\r\n{content}");
                }
            });
        }
    }

    public class AssmeblyOption
    {
        [Option('f', "AssemblyInfoFile", Required = true, HelpText = "The assmebly info file")]
        public string AssemblyInfoFile { set; get; }

        //version-format
        [Option('r', "VersionFormat", Required = false, HelpText = "The version format regex, default is Version = \\\"(\\d+.\\d+.\\d+)\\\";")]
        public string VersionFormatRegex { set; get; }
    }
}