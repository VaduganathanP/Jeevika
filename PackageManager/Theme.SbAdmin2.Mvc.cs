using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace Jeevika.PackageManager
{
    public class Theme_SBAdmin2_Mvc
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");
            XmlElement xmlElement = packageConfigFile.CreateElement("package");
            xmlElement.SetAttribute("id", "Theme.SBAdmin2.Mvc");
            xmlElement.SetAttribute("version", "4.0.6");
            xmlElement.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement);
            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Copying related files
            string packagePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "StaticFiles", "PackageManager", "Theme.SBAdmin2.Mvc").Replace("file:\\", "");
            List<Tuple<string, string>> fileSourceAndDestinationPath = new List<Tuple<string, string>>();
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "sb-admin-2.css"), Path.Combine(applicationPath, "Content", "sb-admin-2.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "sb-admin-2.min.css"), Path.Combine(applicationPath, "Content", "sb-admin-2.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "sb-admin-2.js"), Path.Combine(applicationPath, "Scripts", "sb-admin-2.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "sb-admin-2.min.js"), Path.Combine(applicationPath, "Scripts", "sb-admin-2.min.js")));
            foreach (Tuple<string, string> tuple in fileSourceAndDestinationPath)
                File.Copy(tuple.Item1, tuple.Item2, true);

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlElement xmlElement1 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement1.SetAttribute("Include", Path.Combine("Content", "sb-admin-2.css"));
            XmlElement xmlElement2 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement2.SetAttribute("Include", Path.Combine("Content", "sb-admin-2.min.css"));
            XmlElement xmlElement3 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement3.SetAttribute("Include", Path.Combine("Scripts", "sb-admin-2.js"));
            XmlElement xmlElement4 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement4.SetAttribute("Include", Path.Combine("Scripts", "sb-admin-2.min.js"));
            XmlNode projEithNode = projRootElement.ChildNodes[8];
            projEithNode.AppendChild(xmlElement1);
            projEithNode.AppendChild(xmlElement2);
            projEithNode.AppendChild(xmlElement3);
            projEithNode.AppendChild(xmlElement4);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));
        }
    }
}
