using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika.PackageManager
{
    public class EntityFramework
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");
            XmlElement xmlElement = packageConfigFile.CreateElement("package");
            xmlElement.SetAttribute("id", "EntityFramework");
            xmlElement.SetAttribute("version", "6.2.0");
            xmlElement.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement);
            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlElement xmlProjElement1 = projectFile.CreateElement("Reference", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement1.SetAttribute("Include", "EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL");
            XmlElement xmlProjElement11 = projectFile.CreateElement("HintPath", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement11.InnerText = "..\\packages\\EntityFramework.6.2.0\\lib\\net45\\EntityFramework.dll";
            xmlProjElement1.AppendChild(xmlProjElement11);

            XmlElement xmlProjElement2 = projectFile.CreateElement("Reference", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement2.SetAttribute("Include", "EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL");
            XmlElement xmlProjElement21 = projectFile.CreateElement("HintPath", projectFile.DocumentElement.NamespaceURI);
            xmlProjElement21.InnerText = "..\\packages\\EntityFramework.6.2.0\\lib\\net45\\EntityFramework.SqlServer.dll";
            xmlProjElement2.AppendChild(xmlProjElement21);

            XmlNode projSixthNode = projRootElement.ChildNodes[6];
            projSixthNode.PrependChild(xmlProjElement1);
            projSixthNode.PrependChild(xmlProjElement2);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            //Adding entry in Web.config file
            XmlDocument webConfigFile = new XmlDocument();
            webConfigFile.Load(Path.Combine(applicationPath, "Web.Config"));
            XmlNode webConfigRootElement = webConfigFile.DocumentElement;

            XmlElement xmlWebConfigElement0 = webConfigFile.CreateElement("connectionStrings", webConfigFile.DocumentElement.NamespaceURI);
            XmlElement xmlWebConfigElement01 = webConfigFile.CreateElement("add", webConfigFile.DocumentElement.NamespaceURI);
            xmlWebConfigElement01.SetAttribute("name", "DefaultConnection");
            xmlWebConfigElement01.SetAttribute("connectionString", "Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\db-" + application.Name + ".mdf;Initial Catalog=db-" + application.Name + ";Integrated Security=True");
            xmlWebConfigElement01.SetAttribute("providerName", "System.Data.SqlClient");
            xmlWebConfigElement0.AppendChild(xmlWebConfigElement01);
            webConfigRootElement.PrependChild(xmlWebConfigElement0);

            XmlElement xmlWebConfigElement1 = webConfigFile.CreateElement("configSections", webConfigFile.DocumentElement.NamespaceURI);
            XmlElement xmlWebConfigElement11 = webConfigFile.CreateElement("section", webConfigFile.DocumentElement.NamespaceURI);
            xmlWebConfigElement11.SetAttribute("name", "entityFramework");
            xmlWebConfigElement11.SetAttribute("type", "System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            xmlWebConfigElement11.SetAttribute("requirePermission", "false");
            xmlWebConfigElement1.AppendChild(xmlWebConfigElement11);
            webConfigRootElement.PrependChild(xmlWebConfigElement1);

            XmlElement xmlWebConfigElement2 = webConfigFile.CreateElement("entityFramework", webConfigFile.DocumentElement.NamespaceURI);

            XmlElement xmlWebConfigElement21 = webConfigFile.CreateElement("defaultConnectionFactory", webConfigFile.DocumentElement.NamespaceURI);
            xmlWebConfigElement21.SetAttribute("type", "System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework");
            XmlElement xmlWebConfigElement211 = webConfigFile.CreateElement("parameters", webConfigFile.DocumentElement.NamespaceURI);
            XmlElement xmlWebConfigElement2111 = webConfigFile.CreateElement("parameter", webConfigFile.DocumentElement.NamespaceURI);
            xmlWebConfigElement2111.SetAttribute("value", "mssqllocaldb");
            XmlElement xmlWebConfigElement22 = webConfigFile.CreateElement("providers", webConfigFile.DocumentElement.NamespaceURI);
            XmlElement xmlWebConfigElement221 = webConfigFile.CreateElement("provider", webConfigFile.DocumentElement.NamespaceURI);
            xmlWebConfigElement221.SetAttribute("invariantName", "System.Data.SqlClient");
            xmlWebConfigElement221.SetAttribute("type", "System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer");

            xmlWebConfigElement2.AppendChild(xmlWebConfigElement21).AppendChild(xmlWebConfigElement211).AppendChild(xmlWebConfigElement2111);
            xmlWebConfigElement2.AppendChild(xmlWebConfigElement22).AppendChild(xmlWebConfigElement221);
            webConfigRootElement.AppendChild(xmlWebConfigElement2);
            webConfigFile.Save(Path.Combine(applicationPath, "Web.Config"));

        }
    }
}
