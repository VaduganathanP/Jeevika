using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika.PackageManager
{
    public class Microsoft_AspNet_Identity_EntityFramework
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");
            XmlElement xmlElement = packageConfigFile.CreateElement("package");
            xmlElement.SetAttribute("id", "Microsoft.AspNet.Identity.EntityFramework");
            xmlElement.SetAttribute("version", "2.2.2");
            xmlElement.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement);
            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSixthNode = projRootElement.ChildNodes[6];
            XmlElement xmlProjElement1 = projectFile.CreateElement("Reference", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement1.SetAttribute("Include", "Microsoft.AspNet.Identity.EntityFramework, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL");
            XmlElement xmlProjElement11 = projectFile.CreateElement("HintPath", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement11.InnerText = "..\\packages\\Microsoft.AspNet.Identity.EntityFramework.2.2.2\\lib\\net45\\Microsoft.AspNet.Identity.EntityFramework.dll";
            xmlProjElement1.AppendChild(xmlProjElement11);
            projSixthNode.AppendChild(xmlProjElement1);

            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));
        }
    }
}
