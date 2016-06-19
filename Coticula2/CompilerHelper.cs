using System;
using System.Configuration;
using System.IO;

namespace Coticula2
{
    internal class CompilerHelper
    {
        public static string Compiler
        {
            get
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, "Coticula2.dll.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);

                AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");
                string MySetting = section.Settings["CscCompiler"].Value;

                string currentCompiler = section.Settings["CscCompiler"].Value;

                //for Unix with Mono (mcs)
                if (IsUnix)
                    currentCompiler = section.Settings["DmcsCompiler"].Value;

                return currentCompiler;
            }
        }

        public static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }
    }
}
