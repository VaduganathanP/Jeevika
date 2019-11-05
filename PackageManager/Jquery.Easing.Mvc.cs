using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Jeevika.PackageManager
{
    public class Jquery_Easing_Mvc
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");
            XmlElement xmlElement = packageConfigFile.CreateElement("package");
            xmlElement.SetAttribute("id", "jquery.easing.mvc");
            xmlElement.SetAttribute("version", "1.4.2");
            xmlElement.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement);
            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Copying related files
            string packagePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "StaticFiles", "PackageManager", "Jquery.Easing.Mvc").Replace("file:\\", "");
            List<Tuple<string, string>> fileSourceAndDestinationPath = new List<Tuple<string, string>>();
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "jquery.easing.compatibility.js"), Path.Combine(applicationPath, "Scripts", "jquery.easing.compatibility.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "jquery.easing.js"), Path.Combine(applicationPath, "Scripts", "jquery.easing.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "jquery.easing.min.js"), Path.Combine(applicationPath, "Scripts", "jquery.easing.min.js")));
            foreach (Tuple<string, string> tuple in fileSourceAndDestinationPath)
                File.Copy(tuple.Item1, tuple.Item2, true);

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlElement xmlElement1 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement1.SetAttribute("Include", Path.Combine("Scripts", "jquery.easing.compatibility.js"));
            XmlElement xmlElement2 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement2.SetAttribute("Include", Path.Combine("Scripts", "jquery.easing.js"));
            XmlElement xmlElement3 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlElement3.SetAttribute("Include", Path.Combine("Scripts", "jquery.easing.min.js"));
            
            XmlNode projEithNode = projRootElement.ChildNodes[8];
            projEithNode.AppendChild(xmlElement1);
            projEithNode.AppendChild(xmlElement2);
            projEithNode.AppendChild(xmlElement3);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));
        }
    }
}
