using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NilNote.Core
{
    class FileUtils
    {
        public static string GetSaveLocationInAppDataFolder()
        {
            var userPath = Environment.GetEnvironmentVariable(
              RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
              "LOCALAPPDATA" : "Home");

            var assy = System.Reflection.Assembly.GetEntryAssembly();
            var companyName = assy.GetCustomAttributes<AssemblyCompanyAttribute>()
              .FirstOrDefault();
            var path = System.IO.Path.Combine(userPath, companyName.Company, "NilNote");

            return path;
        }

    }
}
