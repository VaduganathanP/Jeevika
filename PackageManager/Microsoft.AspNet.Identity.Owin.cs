using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika.PackageManager
{
    public class Microsoft_AspNet_Identity_Owin
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");

            XmlElement xmlElement1 = packageConfigFile.CreateElement("package");
            xmlElement1.SetAttribute("id", "Microsoft.AspNet.Identity.Core");
            xmlElement1.SetAttribute("version", "2.2.2");
            xmlElement1.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement1);

            XmlElement xmlElement2 = packageConfigFile.CreateElement("package");
            xmlElement2.SetAttribute("id", "Microsoft.AspNet.Identity.Owin");
            xmlElement2.SetAttribute("version", "2.2.2");
            xmlElement2.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement2);

            XmlElement xmlElement3 = packageConfigFile.CreateElement("package");
            xmlElement3.SetAttribute("id", "Microsoft.Owin");
            xmlElement3.SetAttribute("version", "4.0.1");
            xmlElement3.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement3);

            XmlElement xmlElement4 = packageConfigFile.CreateElement("package");
            xmlElement4.SetAttribute("id", "Microsoft.Owin.Security");
            xmlElement4.SetAttribute("version", "4.0.1");
            xmlElement4.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement4);

            XmlElement xmlElement5 = packageConfigFile.CreateElement("package");
            xmlElement5.SetAttribute("id", "Microsoft.Owin.Security.Cookies");
            xmlElement5.SetAttribute("version", "4.0.1");
            xmlElement5.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement5);

            XmlElement xmlElement6 = packageConfigFile.CreateElement("package");
            xmlElement6.SetAttribute("id", "Microsoft.Owin.Security.OAuth");
            xmlElement6.SetAttribute("version", "4.0.1");
            xmlElement6.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement6);

            XmlElement xmlElement7 = packageConfigFile.CreateElement("package");
            xmlElement7.SetAttribute("id", "Owin");
            xmlElement7.SetAttribute("version", "1.0");
            xmlElement7.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement7);

            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSixthNode = projRootElement.ChildNodes[6];
            List<Tuple<string, string>> refList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Microsoft.AspNet.Identity.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", "..\\packages\\Microsoft.AspNet.Identity.Core.2.2.2\\lib\\net45\\Microsoft.AspNet.Identity.Core.dll"),
                new Tuple<string, string>("Microsoft.AspNet.Identity.Owin, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL","..\\packages\\Microsoft.AspNet.Identity.Owin.2.2.2\\lib\\net45\\Microsoft.AspNet.Identity.Owin.dll"),
                new Tuple<string, string>("Microsoft.Owin, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL","..\\packages\\Microsoft.Owin.4.0.1\\lib\\net45\\Microsoft.Owin.dll"),
                new Tuple<string, string>("Microsoft.Owin.Security, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL","..\\packages\\Microsoft.Owin.Security.4.0.1\\lib\\net45\\Microsoft.Owin.Security.dll"),
                new Tuple<string, string>("Microsoft.Owin.Security.Cookies, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL","..\\packages\\Microsoft.Owin.Security.Cookies.4.0.1\\lib\\net45\\Microsoft.Owin.Security.Cookies.dll"),
                new Tuple<string, string>("Microsoft.Owin.Security.OAuth, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL","..\\packages\\Microsoft.Owin.Security.OAuth.4.0.1\\lib\\net45\\Microsoft.Owin.Security.OAuth.dll"),
                new Tuple<string, string>("Owin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f0ebd12fd5e55cc5, processorArchitecture=MSIL","..\\packages\\Owin.1.0\\lib\\net40\\Owin.dll"),
            };

            foreach (Tuple<string, string> tpl in refList)
            {
                XmlElement xmlProjElement1 = projectFile.CreateElement("Reference", projectFile.DocumentElement.NamespaceURI);
                xmlProjElement1.SetAttribute("Include", tpl.Item1);
                XmlElement xmlProjElement11 = projectFile.CreateElement("HintPath", projectFile.DocumentElement.NamespaceURI);
                xmlProjElement11.InnerText = tpl.Item2;
                xmlProjElement1.AppendChild(xmlProjElement11);
                projSixthNode.AppendChild(xmlProjElement1);
            }
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            //Adding entry in Web.config file
            XmlDocument webConfigFile = new XmlDocument();
            webConfigFile.Load(Path.Combine(applicationPath, "Web.Config"));
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(webConfigFile.NameTable);
            namespaceManager.AddNamespace("ns", "urn:schemas-microsoft-com:asm.v1");
            XmlNode webConfigRootAssemblyBundingElement = webConfigFile.SelectSingleNode("/configuration/runtime/ns:assemblyBinding", namespaceManager);

            XmlElement xmlWebConfigElement3 = webConfigFile.CreateElement("dependentAssembly", webConfigRootAssemblyBundingElement.NamespaceURI);
            XmlElement xmlWebConfigElement31 = webConfigFile.CreateElement("assemblyIdentity", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement31.SetAttribute("name", "Microsoft.Owin");
            xmlWebConfigElement31.SetAttribute("publicKeyToken", "31bf3856ad364e35");
            XmlElement xmlWebConfigElement32 = webConfigFile.CreateElement("bindingRedirect", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement32.SetAttribute("oldVersion", "0.0.0.0-4.0.1.0");
            xmlWebConfigElement32.SetAttribute("newVersion", "4.0.1.0");
            xmlWebConfigElement3.AppendChild(xmlWebConfigElement31);
            xmlWebConfigElement3.AppendChild(xmlWebConfigElement32);
            webConfigRootAssemblyBundingElement.PrependChild(xmlWebConfigElement3);

            XmlElement xmlWebConfigElement1 = webConfigFile.CreateElement("dependentAssembly", webConfigRootAssemblyBundingElement.NamespaceURI);
            XmlElement xmlWebConfigElement11 = webConfigFile.CreateElement("assemblyIdentity", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement11.SetAttribute("name", "Microsoft.Owin.Security.Cookies");
            xmlWebConfigElement11.SetAttribute("publicKeyToken", "31bf3856ad364e35");
            XmlElement xmlWebConfigElement12 = webConfigFile.CreateElement("bindingRedirect", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement12.SetAttribute("oldVersion", "0.0.0.0-4.0.1.0");
            xmlWebConfigElement12.SetAttribute("newVersion", "4.0.1.0");
            xmlWebConfigElement1.AppendChild(xmlWebConfigElement11);
            xmlWebConfigElement1.AppendChild(xmlWebConfigElement12);
            webConfigRootAssemblyBundingElement.PrependChild(xmlWebConfigElement1);

            XmlElement xmlWebConfigElement2 = webConfigFile.CreateElement("dependentAssembly", webConfigRootAssemblyBundingElement.NamespaceURI);
            XmlElement xmlWebConfigElement21 = webConfigFile.CreateElement("assemblyIdentity", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement21.SetAttribute("name", "Microsoft.Owin.Security.OAuth");
            xmlWebConfigElement21.SetAttribute("publicKeyToken", "31bf3856ad364e35");
            XmlElement xmlWebConfigElement22 = webConfigFile.CreateElement("bindingRedirect", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement22.SetAttribute("oldVersion", "0.0.0.0-4.0.1.0");
            xmlWebConfigElement22.SetAttribute("newVersion", "4.0.1.0");
            xmlWebConfigElement2.AppendChild(xmlWebConfigElement21);
            xmlWebConfigElement2.AppendChild(xmlWebConfigElement22);
            webConfigRootAssemblyBundingElement.PrependChild(xmlWebConfigElement2);

            XmlElement xmlWebConfigElement0 = webConfigFile.CreateElement("dependentAssembly", webConfigRootAssemblyBundingElement.NamespaceURI);
            XmlElement xmlWebConfigElement01 = webConfigFile.CreateElement("assemblyIdentity", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement01.SetAttribute("name", "Microsoft.Owin.Security");
            xmlWebConfigElement01.SetAttribute("publicKeyToken", "31bf3856ad364e35");
            XmlElement xmlWebConfigElement02 = webConfigFile.CreateElement("bindingRedirect", webConfigRootAssemblyBundingElement.NamespaceURI);
            xmlWebConfigElement02.SetAttribute("oldVersion", "0.0.0.0-4.0.1.0");
            xmlWebConfigElement02.SetAttribute("newVersion", "4.0.1.0");
            xmlWebConfigElement0.AppendChild(xmlWebConfigElement01);
            xmlWebConfigElement0.AppendChild(xmlWebConfigElement02);
            webConfigRootAssemblyBundingElement.PrependChild(xmlWebConfigElement0);

            webConfigFile.Save(Path.Combine(applicationPath, "Web.Config"));

        }
    }
}
