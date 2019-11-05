using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Application_sln
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("", "");
            f.WriteLine("Microsoft Visual Studio Solution File, Format Version 12.00", "");
            f.WriteLine("# Visual Studio Version 16", "");
            f.WriteLine("VisualStudioVersion = 16.0.29009.5", "");
            f.WriteLine("MinimumVisualStudioVersion = 10.0.40219.1", "");
            f.WriteLine("Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}\", \"{0}\\{0}.csproj\", \"{{7C488288-A112-4E17-9C82-0E263A6F7B85}}\"", application.Name);
            f.WriteLine("EndProject", "");
            f.WriteLine("Global", "");
            f.WriteLine("	GlobalSection(SolutionConfigurationPlatforms) = preSolution", "");
            f.WriteLine("		Debug|Any CPU = Debug|Any CPU", "");
            f.WriteLine("		Release|Any CPU = Release|Any CPU", "");
            f.WriteLine("	EndGlobalSection", "");
            f.WriteLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution", "");
            f.WriteLine("		{{7C488288-A112-4E17-9C82-0E263A6F7B85}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU", "");
            f.WriteLine("		{{7C488288-A112-4E17-9C82-0E263A6F7B85}}.Debug|Any CPU.Build.0 = Debug|Any CPU", "");
            f.WriteLine("		{{7C488288-A112-4E17-9C82-0E263A6F7B85}}.Release|Any CPU.ActiveCfg = Release|Any CPU", "");
            f.WriteLine("		{{7C488288-A112-4E17-9C82-0E263A6F7B85}}.Release|Any CPU.Build.0 = Release|Any CPU", "");
            f.WriteLine("	EndGlobalSection", "");
            f.WriteLine("	GlobalSection(SolutionProperties) = preSolution", "");
            f.WriteLine("		HideSolutionNode = FALSE", "");
            f.WriteLine("	EndGlobalSection", "");
            f.WriteLine("	GlobalSection(ExtensibilityGlobals) = postSolution", "");
            f.WriteLine("		SolutionGuid = {{0BEA66FA-86D1-4009-B5D5-8126147130A7}}", "");
            f.WriteLine("	EndGlobalSection", "");
            f.WriteLine("EndGlobal", "");
        }
    }
}
