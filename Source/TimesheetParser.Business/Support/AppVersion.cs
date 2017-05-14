using System;
using System.Linq;
using System.Reflection;

namespace TimesheetParser.Business.Support
{
    /// <summary>
    /// Contains info about app version.
    /// </summary>
    public class AppVersion
    {
        /// <summary>
        /// Short version. Contains major, minor, patch.
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// Long version. Contains Git branch and changeset.
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// Returns app version using reflection.
        /// </summary>
        /// <returns></returns>
        public static AppVersion Get()
        {
            var executingAssembly = typeof(AppVersion).GetTypeInfo().Assembly;

            var result = new AppVersion
            {
                FileVersion = executingAssembly.GetName().Version.ToString()
            };

            var attribute = executingAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault()
                as AssemblyInformationalVersionAttribute;
            result.ProductVersion = attribute?.InformationalVersion;

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.Format(@"{{""fileVersion"":""{0}"",""productVersion"":""{1}"" }}", FileVersion, ProductVersion);
        }
    }
}
