using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Jeevika.PackageManager
{
    internal class Microsoft_Owin_Host_SystemWeb
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");

            XmlElement xmlElement1 = packageConfigFile.CreateElement("package");
            xmlElement1.SetAttribute("id", "Microsoft.Owin.Host.SystemWeb");
            xmlElement1.SetAttribute("version", "4.0.1");
            xmlElement1.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement1);

            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSixthNode = projRootElement.ChildNodes[6];
            List<Tuple<string, string>> refList = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Microsoft.Owin.Host.SystemWeb, Version=4.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", "..\\packages\\Microsoft.Owin.Host.SystemWeb.4.0.1\\lib\\net45\\Microsoft.Owin.Host.SystemWeb.dll"),
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
        }
    }
}