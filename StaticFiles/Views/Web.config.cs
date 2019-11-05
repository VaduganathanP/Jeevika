using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Views
{
    public class Web_config
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<?xml version=\"1.0\"?>", "");
            f.WriteLine("", "");
            f.WriteLine("<configuration>", "");
            f.WriteLine("  <configSections>", "");
            f.WriteLine("    <sectionGroup name=\"system.web.webPages.razor\" type=\"System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">", "");
            f.WriteLine("      <section name=\"host\" type=\"System.Web.WebPages.Razor.Configuration.HostSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" />", "");
            f.WriteLine("      <section name=\"pages\" type=\"System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" />", "");
            f.WriteLine("    </sectionGroup>", "");
            f.WriteLine("  </configSections>", "");
            f.WriteLine("", "");
            f.WriteLine("  <system.web.webPages.razor>", "");
            f.WriteLine("    <host factoryType=\"System.Web.Mvc.MvcWebRazorHostFactory, System.Web.Mvc, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />", "");
            f.WriteLine("    <pages pageBaseType=\"System.Web.Mvc.WebViewPage\">", "");
            f.WriteLine("      <namespaces>", "");
            f.WriteLine("        <add namespace=\"System.Web.Mvc\" />", "");
            f.WriteLine("        <add namespace=\"System.Web.Mvc.Ajax\" />", "");
            f.WriteLine("        <add namespace=\"System.Web.Mvc.Html\" />", "");
            f.WriteLine("        <add namespace=\"System.Web.Optimization\"/>", "");
            f.WriteLine("        <add namespace=\"System.Web.Routing\" />", "");
            f.WriteLine("        <add namespace=\"{0}\" />", application.Name);
            f.WriteLine("      </namespaces>", "");
            f.WriteLine("    </pages>", "");
            f.WriteLine("  </system.web.webPages.razor>", "");
            f.WriteLine("", "");
            f.WriteLine("  <appSettings>", "");
            f.WriteLine("    <add key=\"webpages:Enabled\" value=\"false\" />", "");
            f.WriteLine("  </appSettings>", "");
            f.WriteLine("", "");
            f.WriteLine("  <system.webServer>", "");
            f.WriteLine("    <handlers>", "");
            f.WriteLine("      <remove name=\"BlockViewHandler\"/>", "");
            f.WriteLine("      <add name=\"BlockViewHandler\" path=\"*\" verb=\"*\" preCondition=\"integratedMode\" type=\"System.Web.HttpNotFoundHandler\" />", "");
            f.WriteLine("    </handlers>", "");
            f.WriteLine("  </system.webServer>", "");
            f.WriteLine("", "");
            f.WriteLine("  <system.web>", "");
            f.WriteLine("    <compilation>", "");
            f.WriteLine("      <assemblies>", "");
            f.WriteLine("        <add assembly=\"System.Web.Mvc, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" />", "");
            f.WriteLine("      </assemblies>", "");
            f.WriteLine("    </compilation>", "");
            f.WriteLine("  </system.web>", "");
            f.WriteLine("</configuration>", "");

        }
    }
}
