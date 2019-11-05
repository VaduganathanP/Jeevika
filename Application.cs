using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika
{
    public class Application : Base
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public List<Entity> CommonEntities { get; set; } = new List<Entity>();
        public Entity AddEntity(Entity model)
        {
            if (string.IsNullOrEmpty(model.ListingScreenTitle))
                model.ListingScreenTitle = model.DisplayName + " List";
            if (model.DetailScreenTitle == string.Empty)
                model.DetailScreenTitle = model.DisplayName + " Detail";
            if (model.AddScreenTitle == string.Empty)
                model.AddScreenTitle = "Add " + model.DisplayName;
            if (model.EditScreenTitle == string.Empty)
                model.EditScreenTitle = "Edit " + model.DisplayName;

            Entities.Add(model);
            return model;
        }

        public void Generate(string projectDir)
        {
            var applicationPath = Path.Combine(projectDir, Name);
            base.CreateDirectory(projectDir);

            AddCommonEntities();
            GenerateEntityFiles(applicationPath);
            GenerateStaticFiles(applicationPath, projectDir);
            CopyStaticFiles(applicationPath);
            GenerateProjectFile(applicationPath);

            Theme.SBAdmin2.Implement(applicationPath, this);

            PackageManager.EntityFramework.Install(applicationPath, this);

            PackageManager.Microsoft_AspNet_Identity_Owin.Install(applicationPath, this);

            PackageManager.Microsoft_AspNet_Identity_EntityFramework.Install(applicationPath, this);

            PackageManager.Microsoft_Owin_Host_SystemWeb.Install(applicationPath, this);

            CreateApplicationDbContextAndInitializerAndIdentityModel(applicationPath);

            GenerateWebAuthorizationModule(applicationPath);

            //ResetNuget(projectDir, applicationPath);

            UpdateProjectFileForGeneratedEntities(applicationPath);
        }

        private void AddCommonEntities()
        {
            Entity fileEntity = new Entity() { Name = "File", DisplayName = "File", AddScreenTitle = "Add File", EditScreenTitle = "Edit File", DetailScreenTitle = "File Detail", ListingScreenTitle = "File List", DisplayInDashBoardMenu = false, IsViewRequired = false };
            fileEntity.Properties.Add(new Property() { Name = "Name", DisplayName = "File Name", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true });
            fileEntity.Properties.Add(new Property() { Name = "ContentType", DisplayName = "Content Type", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true });
            fileEntity.Properties.Add(new Property() { Name = "ContentLength", DisplayName = "Content Length", VariableType = eVariableType.INT, IsRequired = true, IsVisibleInListingScreen = true });
            fileEntity.Properties.Add(new Property() { Name = "Data", DisplayName = "Data", VariableType = eVariableType.BYTE_ARRAY, IsRequired = true, IsVisibleInListingScreen = false });

            this.CommonEntities.Add(fileEntity);
        }

        private void GenerateWebAuthorizationModule(string applicationPath)
        {
            WebAuthorization webAuthorization = new WebAuthorization();
            webAuthorization.Create(applicationPath, this);
            webAuthorization.CreateLoginModule(applicationPath, this);
            webAuthorization.CreateRegisterModule(applicationPath, this);
            webAuthorization.CreateAccountModule(applicationPath, this);

        }

        public void CreateApplicationDbContextAndInitializerAndIdentityModel(string applicationPath)
        {
            Model model = new Model();
            model.CreateApplicationDbContext(Path.Combine(applicationPath, "Models"), this);
            model.CreateApplicationDbInitializer(Path.Combine(applicationPath, "Models"), this);
            model.CreateIdentityModel(Path.Combine(applicationPath, "Models"), this);
        }

        private void UpdateProjectFileForGeneratedEntities(string applicationPath)
        {
            //Create App_Data folder
            base.CreateDirectory(Path.Combine(applicationPath, "App_Data"));
            base.CreateDirectory(Path.Combine(applicationPath, "bin", "App_Data"));

            //Add entry in csproj file
            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];
            XmlNode projEighthNode = projRootElement.ChildNodes[8];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Models", "BaseModel.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);

            XmlElement xmlBaseControllerElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseControllerElement.SetAttribute("Include", Path.Combine("Controllers", "BaseController.cs"));
            projSeventhNode.AppendChild(xmlBaseControllerElement);

            XmlElement xmlFileControllerElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlFileControllerElement.SetAttribute("Include", Path.Combine("Controllers", "FileController.cs"));
            projSeventhNode.AppendChild(xmlFileControllerElement);

            //Model\ApplicationDbContext.cs
            XmlElement xmlApplicationDbContextElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlApplicationDbContextElement.SetAttribute("Include", Path.Combine("Models", "ApplicationDbContext.cs"));
            projSeventhNode.AppendChild(xmlApplicationDbContextElement);

            //Model\ApplicationDbInitializer.cs
            XmlElement xmlApplicationDbInitializerElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlApplicationDbInitializerElement.SetAttribute("Include", Path.Combine("Models", "ApplicationDbInitializer.cs"));
            projSeventhNode.AppendChild(xmlApplicationDbInitializerElement);

            //Model\IdentityModel.cs
            XmlElement xmlIdentityModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlIdentityModelElement.SetAttribute("Include", Path.Combine("Models", "IdentityModel.cs"));
            projSeventhNode.AppendChild(xmlIdentityModelElement);

            foreach (Entity entity in Entities)
            {
                XmlElement xmlElement1 = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
                xmlElement1.SetAttribute("Include", Path.Combine("Models", entity.Name + "Model.cs"));
                projSeventhNode.AppendChild(xmlElement1);

                if (entity.IsViewRequired)
                {
                    XmlElement xmlElement2 = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
                    xmlElement2.SetAttribute("Include", Path.Combine("Controllers", entity.Name + "Controller.cs"));
                    projSeventhNode.AppendChild(xmlElement2);

                    XmlElement xmlElement3 = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
                    xmlElement3.SetAttribute("Include", Path.Combine("ViewModels", entity.Name + "ViewModel.cs"));
                    projSeventhNode.AppendChild(xmlElement3);

                    XmlElement xmlElement4 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
                    xmlElement4.SetAttribute("Include", Path.Combine("Views", entity.Name, "Index.cshtml"));
                    projEighthNode.AppendChild(xmlElement4);

                    XmlElement xmlElement5 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
                    xmlElement5.SetAttribute("Include", Path.Combine("Views", entity.Name, "Add.cshtml"));
                    projEighthNode.AppendChild(xmlElement5);

                    XmlElement xmlElement6 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
                    xmlElement6.SetAttribute("Include", Path.Combine("Views", entity.Name, "Edit.cshtml"));
                    projEighthNode.AppendChild(xmlElement6);

                    XmlElement xmlElement7 = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
                    xmlElement7.SetAttribute("Include", Path.Combine("Views", entity.Name, "Detail.cshtml"));
                    projEighthNode.AppendChild(xmlElement7);
                }
            }

            projectFile.Save(Path.Combine(applicationPath, Name + ".csproj"));
        }

        private void ResetNuget(string projectDir, string applicationDir)
        {
            var proc = new Process();
            try
            {
                proc.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "nuget", "nuget.exe").Replace("file:\\", "");
                proc.StartInfo.Arguments = "restore " + Path.Combine(projectDir, Name + ".sln");
                proc.Start();
                proc.WaitForExit();
                var exitCode = proc.ExitCode;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
        }

        private void GenerateProjectFile(string applicationPath)
        {
            var projFilePath = Path.Combine(applicationPath, Name + ".csproj");
            StaticFiles.Application_csproj.WriteContent(base.CreateFile(projFilePath), this);
            base.Dispose();
        }

        private void CopyStaticFiles(string applicationPath)
        {
            #region Copy all content Directory files
            var sourceContentPath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.CodeBase), "StaticFiles", "Content").Replace("file:\\", "");
            var destinationContentPath = Path.Combine(applicationPath, "Content");
            base.CreateDirectory(destinationContentPath);
            foreach (string dirPath in Directory.GetDirectories(sourceContentPath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceContentPath, destinationContentPath));

            foreach (string newPath in Directory.GetFiles(sourceContentPath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourceContentPath, destinationContentPath), true);
            #endregion

            #region Copy all Scripts Directory files
            var sourceScriptsPath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.CodeBase), "StaticFiles", "Scripts").Replace("file:\\", "");
            var destinationScriptsPath = Path.Combine(applicationPath, "Scripts");
            base.CreateDirectory(destinationScriptsPath);

            foreach (string dirPath in Directory.GetDirectories(sourceScriptsPath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceScriptsPath, destinationScriptsPath));

            foreach (string newPath in Directory.GetFiles(sourceScriptsPath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourceScriptsPath, destinationScriptsPath), true);
            #endregion

            #region Copy all image Directory files
            var sourceImagePath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.CodeBase), "StaticFiles", "Images").Replace("file:\\", "");
            var destinationImagePath = Path.Combine(applicationPath, "Images");
            base.CreateDirectory(destinationImagePath);

            foreach (string dirPath in Directory.GetDirectories(sourceImagePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceImagePath, destinationImagePath));

            foreach (string newPath in Directory.GetFiles(sourceImagePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourceImagePath, destinationImagePath), true);
            #endregion

            #region Copy Favicon.io
            var sourceFaviconIcoPath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.CodeBase), "StaticFiles", "favicon.ico").Replace("file:\\", "");
            var destinationFaviconIcoPath = Path.Combine(applicationPath, "favicon.ico");
            File.Copy(sourceFaviconIcoPath, destinationFaviconIcoPath, true);
            #endregion
        }



        private void GenerateEntityFiles(string applicationPath)
        {
            //foreach (Entity entity in Entities)
            //{
            //    foreach (Entity entity1 in Entities)
            //    {
            //        if (!(entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity1).Count() > 0 && entity1.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0))
            //        {
            //            if (entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity1).Count() > 0)
            //            {
            //                entity1.HasForeignKeyOF(new RelatedEntity() { Name = entity.Name, DisplayName = entity.DisplayName, Entity = entity, IsVisibleInListingScreen = false, IsRequired = true });
            //            }
            //        }
            //    }
            //}

            foreach (Entity entity in CommonEntities)
            {
                Entities.Add(entity);
            }

            foreach (Entity entity in Entities)
            {
                if (!entity.IsRelationShipTable)
                {
                    //if (entity.Properties.Count(o => o.Name == "DisplayName") == 0)
                    //{
                    //    if (entity.Properties.Count(o => o.Name == "Name") == 0)
                    //        entity.Properties.Insert(0, new Property() { Name = "DisplayName", DisplayName = "Display Name", IsRequired = true, IsVisibleInListingScreen = true, VariableType = eVariableType.STRING, Order = 2 });
                    //    else
                    //    {
                    //        var nameProperty = entity.Properties.FirstOrDefault(o => o.Name == "Name");
                    //        var namePosition = entity.Properties.IndexOf(nameProperty);
                    //        entity.Properties.Insert(namePosition + 1, new Property() { Name = "DisplayName", DisplayName = "Display Name", IsRequired = true, IsVisibleInListingScreen = true, VariableType = eVariableType.STRING, Order = 2 });
                    //    }
                    //}
                    if (entity.Properties.Count(o => o.Name == "Name") == 0)
                        entity.Properties.Insert(0, new Property() { Name = "Name", DisplayName = "Name", IsRequired = true, IsVisibleInListingScreen = true, VariableType = eVariableType.STRING, Order = 1 });
                }

                entity.Generate(applicationPath, this);
            }

            Model model = new Model();
            var modelDirectory = Path.Combine(applicationPath, "Models");
            model.CreateBaseModel(modelDirectory, this);

            Controller controller = new Controller();
            var controllerDirectory = Path.Combine(applicationPath, "Controllers");
            base.CreateDirectory(controllerDirectory);
            controller.CreateBaseController(controllerDirectory, this);
            controller.CreateFileController(controllerDirectory, this);
        }

        private void GenerateStaticFiles(string applicationPath, string projectDir)
        {
            #region Application.sln
            var solutionFilePath = Path.Combine(projectDir, Name + ".sln");
            StaticFiles.Application_sln.WriteContent(base.CreateFile(solutionFilePath), this);
            #endregion

            #region Global.asax
            var globalAsaxFilePath = Path.Combine(applicationPath, "Global.asax");
            StaticFiles.Global_asax.WriteContent(base.CreateFile(globalAsaxFilePath), this);
            #endregion

            #region Global.asax.cs
            var globalAsaxCsFilePath = Path.Combine(applicationPath, "Global.asax.cs");
            StaticFiles.Global_asax_cs.WriteContent(base.CreateFile(globalAsaxCsFilePath), this);
            #endregion

            #region packages.config
            var packagesConfigFilePath = Path.Combine(applicationPath, "packages.config");
            StaticFiles.Packages_config.WriteContent(base.CreateFile(packagesConfigFilePath), this);
            #endregion

            #region Web.config
            var webConfigFilePath = Path.Combine(applicationPath, "Web.config");
            StaticFiles.Web_config.WriteContent(base.CreateFile(webConfigFilePath), this);
            #endregion

            #region Web.Debug.config
            var webDebugConfigFilePath = Path.Combine(applicationPath, "Web.Debug.config");
            StaticFiles.Web_Debug_config.WriteContent(base.CreateFile(webDebugConfigFilePath), this);
            #endregion

            #region Web.Release.config
            var webReleaseConfigFilePath = Path.Combine(applicationPath, "Web.Release.config");
            StaticFiles.Web_Release_config.WriteContent(base.CreateFile(webReleaseConfigFilePath), this);
            #endregion

            #region Views\Web.config
            var viewWebConfigFilePath = Path.Combine(applicationPath, "Views", "Web.config");
            StaticFiles.Views.Web_config.WriteContent(base.CreateFile(viewWebConfigFilePath), this);
            #endregion

            #region Views\_ViewStart.cshtml
            var viewStartCshtmlFilePath = Path.Combine(applicationPath, "Views", "_ViewStart.cshtml");
            StaticFiles.Views.ViewStart_cshtml.WriteContent(base.CreateFile(viewStartCshtmlFilePath), this);
            #endregion

            var controllersDirPath = Path.Combine(applicationPath, "Controllers");
            base.CreateDirectory(controllersDirPath);
            #region Controllers\HomeController.cs
            var homeControllerFilePath = Path.Combine(applicationPath, "Controllers", "HomeController.cs");
            StaticFiles.Controllers.HomeController_cs.WriteContent(base.CreateFile(homeControllerFilePath), this);
            #endregion

            var converterDirPath = Path.Combine(applicationPath, "Converters");
            base.CreateDirectory(converterDirPath);
            #region Converters\DateTimeConverter.cs
            var dateTimeConverterFilePath = Path.Combine(applicationPath, "Converters", "DateTimeConverter.cs");
            StaticFiles.Converters.DateTimeConverter_cs.WriteContent(base.CreateFile(dateTimeConverterFilePath), this);
            #endregion

            var viewsHomeDirPath = Path.Combine(applicationPath, "Views", "Home");
            base.CreateDirectory(viewsHomeDirPath);
            #region Views\Home\Index.cshtml
            var viewHomeIndexCshtmlFilePath = Path.Combine(applicationPath, "Views", "Home", "Index.cshtml");
            StaticFiles.Views.Home.Index_cshtml.WriteContent(base.CreateFile(viewHomeIndexCshtmlFilePath), this);
            #endregion

            var viewsSharedDirPath = Path.Combine(applicationPath, "Views", "Shared");
            base.CreateDirectory(viewsSharedDirPath);
            #region Views\Shared\Error.cshtml
            var viewSharedErrorCshtmlFilePath = Path.Combine(applicationPath, "Views", "Shared", "Error.cshtml");
            StaticFiles.Views.Shared.Error_cshtml.WriteContent(base.CreateFile(viewSharedErrorCshtmlFilePath), this);
            #endregion
            #region Views\Shared\_Layout.cshtml
            var viewSharedLayoutCshtmlFilePath = Path.Combine(applicationPath, "Views", "Shared", "_Layout.cshtml");
            StaticFiles.Views.Shared.Layout_cshtml.WriteContent(base.CreateFile(viewSharedLayoutCshtmlFilePath), this);
            #endregion

            var propertiesDirPath = Path.Combine(applicationPath, "Properties");
            base.CreateDirectory(propertiesDirPath);
            #region Properties\AssemblyInfo.cs
            var assemblyInfoFilePath = Path.Combine(propertiesDirPath, "AssemblyInfo.cs");
            StaticFiles.Properties.AssemblyInfo.WriteContent(base.CreateFile(assemblyInfoFilePath), this);
            #endregion

            var appstartDirPath = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(appstartDirPath);
            #region App_Start\BundleConfig.cs
            var bundleConfigFilePath = Path.Combine(appstartDirPath, "BundleConfig.cs");
            StaticFiles.App_Start.BundleConfig.WriteContent(base.CreateFile(bundleConfigFilePath), this);
            #endregion
            #region App_Start\FilterConfig.cs
            var filterConfigFilePath = Path.Combine(appstartDirPath, "FilterConfig.cs");
            StaticFiles.App_Start.FilterConfig.WriteContent(base.CreateFile(filterConfigFilePath), this);
            #endregion
            #region App_Start\RouteConfig.cs
            var routeConfigFilePath = Path.Combine(appstartDirPath, "RouteConfig.cs");
            StaticFiles.App_Start.RouteConfig.WriteContent(base.CreateFile(routeConfigFilePath), this);
            #endregion

            base.Dispose();
        }

    }
}
