// Copyright (c) Ubisoft. All Rights Reserved.
// Licensed under the Apache 2.0 License. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;

namespace Sharpmake
{
    public abstract class NinjaMakeCommandGenerator
    {
        public enum BuildType
        {
            Build,
            Rebuild
        };

        public abstract string GetCommand(BuildType buildType, Sharpmake.Project.Configuration conf, string ninjaArguments);
    }


    public static class NinjaSettings
    {
        public const string NinjaConfigFileExtension = ".ninja";
        public const string MasterNinjaFileName = "build";

        /// <summary>
        /// Full path to the %WINDIR% directory.
        /// Usually equals to `C:\WINDOWS`.
        /// </summary>
        public static string SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

        /// <summary>
        /// Full path to the system dll folder where the ucrtbase.dll and api-ms-win-*.dll can be found.
        /// If left null, dlls will be searched in the Redist\ucrt\DLLs\x64 subfolder of the WinSDK10 indicated in the KitsRootPaths.
        /// </summary>
        public static string SystemDllRoot = null;

        /// <summary>
        /// Full path under which files and folders are considered part of the workspace and can be expressed as relative to one another.
        /// If left null, project.RootPath will be used instead.
        /// </summary>
        public static string WorkspaceRoot = null;

        /// <summary>
        /// Additional settings to add to the global settings node.
        /// </summary>
        public static readonly IList<string> AdditionalGlobalSettings = new List<string>();

        /// <summary>
        /// Additional environment variables to add to the global environment settings node (key, value)
        /// </summary>
        public static readonly IDictionary<string, string> AdditionalGlobalEnvironmentVariables = new Dictionary<string, string>();

        public static bool NinjaVerbose = false;

        /// <summary>
        /// The path to the executable used to start a ninja compilation. This path is relative to the source tree root.
        /// ex: @"tools\ninja\ninja.exe"
        /// </summary>
        public static string NinjaMakeCommand = null; // PLEASE OVERRIDE this in your Sharpmake main

        /// <summary>
        /// Can be set to false to override all FastBuild settings and disable it
        /// </summary>
        public static volatile bool NinjaSupportEnabled = true;

        /// <summary>
        /// Additional settings to add to the Compiler node, keyed by compiler name.
        /// </summary>
        ///
        public static readonly IDictionary<string, List<string>> AdditionalCompilerSettings = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Additional Section referred by a compiler node, keyed by compiler name
        /// </summary>
        public static readonly IDictionary<string, string> AdditionalCompilerPropertyGroups = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Additional custom property groups. Only those referred will be written to the bff files.
        /// </summary>
        public static readonly IDictionary<string, List<string>> AdditionalPropertyGroups = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Custom arguments pass to ninja
        /// </summary>
        public static string NinjaCustomArguments = null;
    }
}
