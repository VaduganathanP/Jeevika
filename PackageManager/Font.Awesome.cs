using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika.PackageManager
{
    public class Font_Awsome
    {
        public static void Install(string applicationPath, Application application)
        {
            //Adding entry in package.config file
            XmlDocument packageConfigFile = new XmlDocument();
            packageConfigFile.Load(Path.Combine(applicationPath, "packages.config"));
            XmlNode rootNode = packageConfigFile.DocumentElement.SelectSingleNode("/packages");
            XmlElement xmlElement = packageConfigFile.CreateElement("package");
            xmlElement.SetAttribute("id", "Font.Awesome");
            xmlElement.SetAttribute("version", "5.9.0");
            xmlElement.SetAttribute("targetFramework", "net472");
            rootNode.AppendChild(xmlElement);
            packageConfigFile.Save(Path.Combine(applicationPath, "packages.config"));

            //Copying related files
            string packagePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "StaticFiles", "PackageManager", "Font.Awesome").Replace("file:\\", "");
            List<Tuple<string, string>> fileSourceAndDestinationPath = new List<Tuple<string, string>>();
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "brands.css"), Path.Combine(applicationPath, "Content", "brands.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "brands.min.css"), Path.Combine(applicationPath, "Content", "brands.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "fontawesome-all.css"), Path.Combine(applicationPath, "Content", "fontawesome-all.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "fontawesome-all.min.css"), Path.Combine(applicationPath, "Content", "fontawesome-all.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "fontawesome.css"), Path.Combine(applicationPath, "Content", "fontawesome.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "fontawesome.min.css"), Path.Combine(applicationPath, "Content", "fontawesome.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "regular.css"), Path.Combine(applicationPath, "Content", "regular.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "regular.min.css"), Path.Combine(applicationPath, "Content", "regular.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "solid.css"), Path.Combine(applicationPath, "Content", "solid.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "solid.min.css"), Path.Combine(applicationPath, "Content", "solid.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "svg-with-js.css"), Path.Combine(applicationPath, "Content", "svg-with-js.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "svg-with-js.min.css"), Path.Combine(applicationPath, "Content", "svg-with-js.min.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "v4-shims.css"), Path.Combine(applicationPath, "Content", "v4-shims.css")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Content", "v4-shims.min.css"), Path.Combine(applicationPath, "Content", "v4-shims.min.css")));

            if (!Directory.Exists(Path.Combine(applicationPath, "Scripts", "fontawesome")))
                Directory.CreateDirectory(Path.Combine(applicationPath, "Scripts", "fontawesome"));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "all.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "all.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "all.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "all.min.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "brands.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "brands.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "brands.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "brands.min.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "fontawesome.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "fontawesome.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "fontawesome.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "fontawesome.min.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "regular.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "regular.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "regular.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "regular.min.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "solid.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "solid.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "solid.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "solid.min.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "v4-shims.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "v4-shims.js")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "Scripts", "fontawesome", "v4-shims.min.js"), Path.Combine(applicationPath, "Scripts", "fontawesome", "v4-shims.min.js")));

            if (!Directory.Exists(Path.Combine(applicationPath, "webfonts")))
                Directory.CreateDirectory(Path.Combine(applicationPath, "webfonts"));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-brands-400.eot"), Path.Combine(applicationPath, "webfonts", "fa-brands-400.eot")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-brands-400.svg"), Path.Combine(applicationPath, "webfonts", "fa-brands-400.svg")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-brands-400.ttf"), Path.Combine(applicationPath, "webfonts", "fa-brands-400.ttf")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-brands-400.woff"), Path.Combine(applicationPath, "webfonts", "fa-brands-400.woff")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-brands-400.woff2"), Path.Combine(applicationPath, "webfonts", "fa-brands-400.woff2")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-regular-400.eot"), Path.Combine(applicationPath, "webfonts", "fa-regular-400.eot")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-regular-400.svg"), Path.Combine(applicationPath, "webfonts", "fa-regular-400.svg")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-regular-400.ttf"), Path.Combine(applicationPath, "webfonts", "fa-regular-400.ttf")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-regular-400.woff"), Path.Combine(applicationPath, "webfonts", "fa-regular-400.woff")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-regular-400.woff2"), Path.Combine(applicationPath, "webfonts", "fa-regular-400.woff2")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-solid-900.eot"), Path.Combine(applicationPath, "webfonts", "fa-solid-900.eot")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-solid-900.svg"), Path.Combine(applicationPath, "webfonts", "fa-solid-900.svg")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-solid-900.ttf"), Path.Combine(applicationPath, "webfonts", "fa-solid-900.ttf")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-solid-900.woff"), Path.Combine(applicationPath, "webfonts", "fa-solid-900.woff")));
            fileSourceAndDestinationPath.Add(new Tuple<string, string>(Path.Combine(packagePath, "webfonts", "fa-solid-900.woff2"), Path.Combine(applicationPath, "webfonts", "fa-solid-900.woff2")));
            foreach (Tuple<string, string> tuple in fileSourceAndDestinationPath)
                File.Copy(tuple.Item1, tuple.Item2, true);

            //Adding entry in Application.csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlElement xmlElement1 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement2 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement3 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement4 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement5 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement6 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement7 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement8 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement9 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement10 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement11 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement12 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement13 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement14 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement15 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement16 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement17 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement18 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement19 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement20 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement21 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement22 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement23 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement24 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement25 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement26 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement27 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement28 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement29 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement30 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement31 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement32 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement33 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement34 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement35 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement36 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement37 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement38 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement39 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement40 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            XmlElement xmlElement41 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            
            xmlElement1.SetAttribute("Include", Path.Combine("Content", "brands.css"));
            xmlElement2.SetAttribute("Include", Path.Combine("Content", "brands.min.css"));
            xmlElement3.SetAttribute("Include", Path.Combine("Content", "fontawesome-all.css"));
            xmlElement4.SetAttribute("Include", Path.Combine("Content", "fontawesome-all.min.css"));
            xmlElement5.SetAttribute("Include", Path.Combine("Content", "fontawesome.css"));
            xmlElement6.SetAttribute("Include", Path.Combine("Content", "fontawesome.min.css"));
            xmlElement7.SetAttribute("Include", Path.Combine("Content", "regular.css"));
            xmlElement8.SetAttribute("Include", Path.Combine("Content", "regular.min.css"));
            xmlElement9.SetAttribute("Include", Path.Combine("Content", "solid.css"));
            xmlElement10.SetAttribute("Include", Path.Combine("Content", "solid.min.css"));
            xmlElement11.SetAttribute("Include", Path.Combine("Content", "svg-with-js.css"));
            xmlElement12.SetAttribute("Include", Path.Combine("Content", "svg-with-js.min.css"));
            xmlElement13.SetAttribute("Include", Path.Combine("Content", "v4-shims.css"));
            xmlElement14.SetAttribute("Include", Path.Combine("Content", "v4-shims.min.css"));
            xmlElement15.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "all.js"));
            xmlElement16.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "all.min.js"));
            xmlElement17.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "brands.js"));
            xmlElement18.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "brands.min.js"));
            xmlElement19.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "fontawesome.js"));
            xmlElement20.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "fontawesome.min.js"));
            xmlElement21.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "regular.js"));
            xmlElement22.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "regular.min.js"));
            xmlElement23.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "solid.js"));
            xmlElement24.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "solid.min.js"));
            xmlElement25.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "v4-shims.js"));
            xmlElement26.SetAttribute("Include", Path.Combine("Scripts", "fontawesome", "v4-shims.min.js"));
            xmlElement27.SetAttribute("Include", Path.Combine("webfonts", "fa-brands-400.eot"));
            xmlElement28.SetAttribute("Include", Path.Combine("webfonts", "fa-brands-400.svg"));
            xmlElement29.SetAttribute("Include", Path.Combine("webfonts", "fa-brands-400.ttf"));
            xmlElement30.SetAttribute("Include", Path.Combine("webfonts", "fa-brands-400.woff"));
            xmlElement31.SetAttribute("Include", Path.Combine("webfonts", "fa-brands-400.woff2"));
            xmlElement32.SetAttribute("Include", Path.Combine("webfonts", "fa-regular-400.eot"));
            xmlElement33.SetAttribute("Include", Path.Combine("webfonts", "fa-regular-400.svg"));
            xmlElement34.SetAttribute("Include", Path.Combine("webfonts", "fa-regular-400.ttf"));
            xmlElement35.SetAttribute("Include", Path.Combine("webfonts", "fa-regular-400.woff"));
            xmlElement36.SetAttribute("Include", Path.Combine("webfonts", "fa-regular-400.woff2"));
            xmlElement37.SetAttribute("Include", Path.Combine("webfonts", "fa-solid-900.eot"));
            xmlElement38.SetAttribute("Include", Path.Combine("webfonts", "fa-solid-900.svg"));
            xmlElement39.SetAttribute("Include", Path.Combine("webfonts", "fa-solid-900.ttf"));
            xmlElement40.SetAttribute("Include", Path.Combine("webfonts", "fa-solid-900.woff"));
            xmlElement41.SetAttribute("Include", Path.Combine("webfonts", "fa-solid-900.woff2"));

            XmlNode projEighthNode = projRootElement.ChildNodes[8];
            projEighthNode.AppendChild(xmlElement1);
            projEighthNode.AppendChild(xmlElement2);
            projEighthNode.AppendChild(xmlElement3);
            projEighthNode.AppendChild(xmlElement4);
            projEighthNode.AppendChild(xmlElement5);
            projEighthNode.AppendChild(xmlElement6);
            projEighthNode.AppendChild(xmlElement7);
            projEighthNode.AppendChild(xmlElement8);
            projEighthNode.AppendChild(xmlElement9);
            projEighthNode.AppendChild(xmlElement10);
            projEighthNode.AppendChild(xmlElement11);
            projEighthNode.AppendChild(xmlElement12);
            projEighthNode.AppendChild(xmlElement13);
            projEighthNode.AppendChild(xmlElement14);
            projEighthNode.AppendChild(xmlElement15);
            projEighthNode.AppendChild(xmlElement16);
            projEighthNode.AppendChild(xmlElement17);
            projEighthNode.AppendChild(xmlElement18);
            projEighthNode.AppendChild(xmlElement19);
            projEighthNode.AppendChild(xmlElement20);
            projEighthNode.AppendChild(xmlElement21);
            projEighthNode.AppendChild(xmlElement22);
            projEighthNode.AppendChild(xmlElement23);
            projEighthNode.AppendChild(xmlElement24);
            projEighthNode.AppendChild(xmlElement25);
            projEighthNode.AppendChild(xmlElement26);
            projEighthNode.AppendChild(xmlElement27);
            projEighthNode.AppendChild(xmlElement28);
            projEighthNode.AppendChild(xmlElement29);
            projEighthNode.AppendChild(xmlElement30);
            projEighthNode.AppendChild(xmlElement31);
            projEighthNode.AppendChild(xmlElement32);
            projEighthNode.AppendChild(xmlElement33);
            projEighthNode.AppendChild(xmlElement34);
            projEighthNode.AppendChild(xmlElement35);
            projEighthNode.AppendChild(xmlElement36);
            projEighthNode.AppendChild(xmlElement37);
            projEighthNode.AppendChild(xmlElement38);
            projEighthNode.AppendChild(xmlElement39);
            projEighthNode.AppendChild(xmlElement40);
            projEighthNode.AppendChild(xmlElement41);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));
        }
    }
}
