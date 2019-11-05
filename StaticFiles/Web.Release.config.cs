using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Web_Release_config
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<?xml version=\"1.0\"?>", "");
            f.WriteLine("", "");
            f.WriteLine("<!-- For more information on using Web.config transformation visit https://go.microsoft.com/fwlink/?LinkId=301874 -->", "");
            f.WriteLine("", "");
            f.WriteLine("<configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\">", "");
            f.WriteLine("  <!--", "");
            f.WriteLine("    In the example below, the \"SetAttributes\" transform will change the value of", "");
            f.WriteLine("    \"connectionString\" to use \"ReleaseSQLServer\" only when the \"Match\" locator", "");
            f.WriteLine("    finds an attribute \"name\" that has a value of \"MyDB\".", "");
            f.WriteLine("", "");
            f.WriteLine("    <connectionStrings>", "");
            f.WriteLine("      <add name=\"MyDB\"", "");
            f.WriteLine("        connectionString=\"Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True\"", "");
            f.WriteLine("        xdt:Transform=\"SetAttributes\" xdt:Locator=\"Match(name)\"/>", "");
            f.WriteLine("    </connectionStrings>", "");
            f.WriteLine("  -->", "");
            f.WriteLine("  <system.web>", "");
            f.WriteLine("    <compilation xdt:Transform=\"RemoveAttributes(debug)\" />", "");
            f.WriteLine("    <!--", "");
            f.WriteLine("      In the example below, the \"Replace\" transform will replace the entire", "");
            f.WriteLine("      <customErrors> section of your Web.config file.", "");
            f.WriteLine("      Note that because there is only one customErrors section under the", "");
            f.WriteLine("      <system.web> node, there is no need to use the \"xdt:Locator\" attribute.", "");
            f.WriteLine("", "");
            f.WriteLine("      <customErrors defaultRedirect=\"GenericError.htm\"", "");
            f.WriteLine("        mode=\"RemoteOnly\" xdt:Transform=\"Replace\">", "");
            f.WriteLine("        <error statusCode=\"500\" redirect=\"InternalError.htm\"/>", "");
            f.WriteLine("      </customErrors>", "");
            f.WriteLine("    -->", "");
            f.WriteLine("  </system.web>", "");
            f.WriteLine("</configuration>", "");
        }
    }
}
