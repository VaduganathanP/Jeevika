using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Properties
{
    public class AssemblyInfo
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System.Reflection;", "");
            f.WriteLine("using System.Runtime.CompilerServices;", "");
            f.WriteLine("using System.Runtime.InteropServices;", "");
            f.WriteLine("", "");
            f.WriteLine("// General Information about an assembly is controlled through the following", "");
            f.WriteLine("// set of attributes. Change these attribute values to modify the information", "");
            f.WriteLine("// associated with an assembly.", "");
            f.WriteLine("[assembly: AssemblyTitle(\"{0}\")]", application.Name);
            f.WriteLine("[assembly: AssemblyDescription(\"\")]", "");
            f.WriteLine("[assembly: AssemblyConfiguration(\"\")]", "");
            f.WriteLine("[assembly: AssemblyCompany(\"\")]", "");
            f.WriteLine("[assembly: AssemblyProduct(\"{0}\")]", application.Name);
            f.WriteLine("[assembly: AssemblyCopyright(\"Copyright © {0} 2019\")]", "Vaduga");
            f.WriteLine("[assembly: AssemblyTrademark(\"\")]", "");
            f.WriteLine("[assembly: AssemblyCulture(\"\")]", "");
            f.WriteLine("", "");
            f.WriteLine("// Setting ComVisible to false makes the types in this assembly not visible", "");
            f.WriteLine("// to COM components.  If you need to access a type in this assembly from", "");
            f.WriteLine("// COM, set the ComVisible attribute to true on that type.", "");
            f.WriteLine("[assembly: ComVisible(false)]", "");
            f.WriteLine("", "");
            f.WriteLine("// The following GUID is for the ID of the typelib if this project is exposed to COM", "");
            f.WriteLine("[assembly: Guid(\"1b65b8c1-1787-4903-a650-98a559dcac15\")]", "");
            f.WriteLine("", "");
            f.WriteLine("// Version information for an assembly consists of the following four values:", "");
            f.WriteLine("//", "");
            f.WriteLine("//      Major Version", "");
            f.WriteLine("//      Minor Version", "");
            f.WriteLine("//      Build Number", "");
            f.WriteLine("//      Revision", "");
            f.WriteLine("//", "");
            f.WriteLine("// You can specify all the values or you can default the Revision and Build Numbers", "");
            f.WriteLine("// by using the '*' as shown below:", "");
            f.WriteLine("[assembly: AssemblyVersion(\"1.0.0.0\")]", "");
            f.WriteLine("[assembly: AssemblyFileVersion(\"1.0.0.0\")]", "");

        }
    }
}
