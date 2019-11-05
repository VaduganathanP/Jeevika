using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Web_config
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            f.WriteLine("<!--", "");
            f.WriteLine("  For more information on how to configure your ASP.NET application, please visit", "");
            f.WriteLine("  https://go.microsoft.com/fwlink/?LinkId=301880", "");
            f.WriteLine("  -->", "");
            f.WriteLine("<configuration>", "");
            f.WriteLine("  <appSettings>", "");
            f.WriteLine("    <add key=\"webpages:Version\" value=\"3.0.0.0\" />", "");
            f.WriteLine("    <add key=\"webpages:Enabled\" value=\"false\" />", "");
            f.WriteLine("    <add key=\"ClientValidationEnabled\" value=\"true\" />", "");
            f.WriteLine("    <add key=\"UnobtrusiveJavaScriptEnabled\" value=\"true\" />", "");
            f.WriteLine("  </appSettings>", "");
            f.WriteLine("  <system.web>", "");
            f.WriteLine("    <compilation debug=\"true\" targetFramework=\"4.7.2\" />", "");
            f.WriteLine("    <httpRuntime targetFramework=\"4.7.2\" />", "");
            f.WriteLine("  </system.web>", "");
            f.WriteLine("  <system.net>", "");
            f.WriteLine("    <mailSettings>", "");
            f.WriteLine("      <smtp from=\"vaduga@outlook.com\">", "");
            f.WriteLine("        <network host=\"smtp-mail.outlook.com\" port=\"587\" userName=\"vaduga@outlook.com\" password=\"\" enableSsl=\"true\" />", "");
            f.WriteLine("      </smtp>", "");
            f.WriteLine("    </mailSettings>", "");
            f.WriteLine("  </system.net>", "");
            f.WriteLine("  <runtime>", "");
            f.WriteLine("    <assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"Antlr3.Runtime\" publicKeyToken=\"eb42632606e9261f\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"0.0.0.0-3.5.0.2\" newVersion=\"3.5.0.2\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"Newtonsoft.Json\" publicKeyToken=\"30ad4fe6b2a6aeed\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"0.0.0.0-12.0.0.0\" newVersion=\"12.0.0.0\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"System.Web.Optimization\" publicKeyToken=\"31bf3856ad364e35\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"1.0.0.0-1.1.0.0\" newVersion=\"1.1.0.0\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"WebGrease\" publicKeyToken=\"31bf3856ad364e35\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"0.0.0.0-1.6.5135.21930\" newVersion=\"1.6.5135.21930\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"System.Web.Helpers\" publicKeyToken=\"31bf3856ad364e35\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"1.0.0.0-3.0.0.0\" newVersion=\"3.0.0.0\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"System.Web.WebPages\" publicKeyToken=\"31bf3856ad364e35\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"1.0.0.0-3.0.0.0\" newVersion=\"3.0.0.0\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("      <dependentAssembly>", "");
            f.WriteLine("        <assemblyIdentity name=\"System.Web.Mvc\" publicKeyToken=\"31bf3856ad364e35\" />", "");
            f.WriteLine("        <bindingRedirect oldVersion=\"1.0.0.0-5.2.7.0\" newVersion=\"5.2.7.0\" />", "");
            f.WriteLine("      </dependentAssembly>", "");
            f.WriteLine("    </assemblyBinding>", "");
            f.WriteLine("  </runtime>", "");
            f.WriteLine("  <system.codedom>", "");
            f.WriteLine("    <compilers>", "");
            f.WriteLine("      <compiler language=\"c#;cs;csharp\" extension=\".cs\" type=\"Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\" warningLevel=\"4\" compilerOptions=\"/langversion:default /nowarn:1659;1699;1701\" />", "");
            f.WriteLine("      <compiler language=\"vb;vbs;visualbasic;vbscript\" extension=\".vb\" type=\"Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=2.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\" warningLevel=\"4\" compilerOptions=\"/langversion:default /nowarn:41008 /define:_MYTYPE=\\&quot;Web\\&quot; /optionInfer+\" />", "");
            f.WriteLine("    </compilers>", "");
            f.WriteLine("  </system.codedom>", "");
            f.WriteLine("</configuration>", "");
        }
    }
}
