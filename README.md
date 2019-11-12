# Jeevika
.Net Framework MVC Application Generator
## Usage https://youtu.be/Ut2BsBPVVhw
1. Using Visual Studio create a console application with .Net Framework 4.7.2
2. Install nuget package https://www.nuget.org/packages/Jeevika
3. In <b>Program.cs</b> try the below code which will generate a blank application
```C#
    using Jeevika;
    class Program
    {
        static void Main(string[] args)
        {
            //Include nuget package Jeevika
            //We will create a simple school management with otion to add students and Grade or Section
            Project project = new Project() { Name = "SchoolManagement", Description = "A simple school management" };
            Application application = project.AddApplication(new Application() { Name = "SchoolManagement", DisplayName= "School Management" });

            //Add Student Entity
            Entity studentEntity = application.AddEntity(new Entity() { Name = "Student", DisplayName = "Student", DisplayInDashBoardMenu = false });
            studentEntity.AddProperty(new Property()
            {
                Name = "Name",
                DisplayName = "Student Name",
                IsRequired = true,
                IsVisibleInListingScreen = true,
                VariableType = eVariableType.STRING
            });
            studentEntity.AddProperty(new Property()
            {
                Name = "Age",
                DisplayName = "Age",
                IsRequired = true,
                IsVisibleInListingScreen = true,
                VariableType = eVariableType.INT
            });

            //Ok lets create Grade entity
            Entity gradeEntity = application.AddEntity(new Entity() { Name = "Grade", DisplayName = "Grade", DisplayInDashBoardMenu = true });
            gradeEntity.AddProperty(new Property()
            {
                Name = "Name",
                DisplayName = "Grade Name",
                IsRequired = true,
                IsVisibleInListingScreen = true,
                VariableType = eVariableType.STRING
            });

            //Now we will crate relation, one grade may contain multiple students
            //Maintain same name as provided to student entity
            gradeEntity.HasMany(new RelatedEntity() { Name = "Student", DisplayName = "Student", Entity = studentEntity, IsHidden = true });

            
            project.Generate();
        }
    }
```
4. The generated code will be available in <b>bin/Debug/AppData/SchoolManagement</b> folder. Here SchoolManagement is the application name provided in Program.cs.
5. Default admin username and password is "vaduga@outlook.com" "#123Vaduga". You can change this as your wish in generated application "Model/ApplicationDbInitializer.cs" file.
6. Application generated uses EntityFramework codefirst, so database will be generated on the first login in <b>localdb</b>. You can man modify connection string in web.config file.

# EXAMPLE
```C#
        static void Main(string[] args)
        {
            int order = 0;

            Project project = new Project() { Name = "ACG", Description = "App Code Generator" };
            Application application = project.AddApplication(new Application() { Name = "ACG", DisplayName = "App Code Generator", IsDebugMode = true });

            order = 0;
            Entity variableTypeEntity = application.AddEntity(new Entity() { Name = "VariableType", DisplayName = "Variable Type", DisplayInDashBoardMenu = true });
            variableTypeEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });

            order = 0;
            Entity relatedEntityEntity = application.AddEntity(new Entity() { Name = "RelatedEntity", DisplayName = "Related Entity", DisplayInDashBoardMenu = false });
            relatedEntityEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "DisplayName", DisplayName = "Display Name", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "IsRequired", DisplayName = "Is Required", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "IsVisibleInListingScreen", DisplayName = "Visible in listing screen", VariableType = eVariableType.BOOL, DefaultValue = "true", IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "ShowInTab", DisplayName = "Show In Tab", VariableType = eVariableType.BOOL, DefaultValue = "true", IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "CascadeOnDelete", DisplayName = "Cascade On Delete", VariableType = eVariableType.BOOL, DefaultValue = "true", IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "IsHidden", DisplayName = "Is Hidden", VariableType = eVariableType.BOOL, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            relatedEntityEntity.AddProperty(new Property() { Name = "DefaultValue", DisplayName = "Default Value", VariableType = eVariableType.STRING, IsVisibleInListingScreen = true, Order = order++ });

            order = 0;
            Entity propertyEntity = application.AddEntity(new Entity() { Name = "Property", DisplayName = "Property", DisplayInDashBoardMenu = false });
            propertyEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            propertyEntity.AddProperty(new Property() { Name = "DisplayName", DisplayName = "Display Name", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            propertyEntity.AddProperty(new Property() { Name = "IsRequired", DisplayName = "Is Required", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            propertyEntity.AddProperty(new Property() { Name = "IsVisibleInListingScreen", DisplayName = "Visible in listing screen", VariableType = eVariableType.BOOL, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            propertyEntity.AddProperty(new Property() { Name = "DefaultValue", DisplayName = "Default Value", VariableType = eVariableType.STRING, IsRequired = false, IsVisibleInListingScreen = false, Order = order++ });
            propertyEntity.AddProperty(new Property() { Name = "Order", DisplayName = "Order", VariableType = eVariableType.INT, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });

            propertyEntity.HasForeignKeyOF(new RelatedEntity() { Name = "VariableType", DisplayName = "Variable Type", Entity = variableTypeEntity, IsVisibleInListingScreen = true, IsRequired = true });

            order = 0;
            Entity propertyGroupEntity = application.AddEntity(new Entity() { Name = "PropertyGroup", DisplayName = "Property Group", DisplayInDashBoardMenu = false });
            propertyGroupEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            propertyGroupEntity.AddProperty(new Property() { Name = "Order", DisplayName = "Order", VariableType = eVariableType.INT, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });

            propertyGroupEntity.HasMany(new RelatedEntity() { Name = "Property", DisplayName = "Property", Entity = propertyEntity, IsRequired = false, IsHidden = true, CascadeOnDelete = false });
            propertyGroupEntity.HasMany(new RelatedEntity() { Name = "ForeignKeyRelation", DisplayName = "Foreign Key Relation", Entity = relatedEntityEntity, IsVisibleInListingScreen = false, IsRequired = false, CascadeOnDelete = false, IsHidden = true });
            propertyGroupEntity.HasMany(new RelatedEntity() { Name = "HasManyRelation", DisplayName = "Has Many Relation", Entity = relatedEntityEntity, IsVisibleInListingScreen = false, IsRequired = false, CascadeOnDelete = false, IsHidden = true });

            order = 0;
            Entity entityEntity = application.AddEntity(new Entity() { Name = "ProjectEntity", DisplayName = "Entity", DisplayInDashBoardMenu = false });
            entityEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "DisplayName", DisplayName = "Display Name", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "DisplayInDashBoardMenu", DisplayName = "Display In DashBoard Menu", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "IsRelationShipTable", DisplayName = "Is RelationShipTable", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "IsViewRequired", DisplayName = "Is View Required", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "true", IsVisibleInListingScreen = false, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "DisplayInAllEntity", DisplayName = "Display In All Entities", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = false, Order = order++ });
            entityEntity.AddProperty(new Property() { Name = "Order", DisplayName = "Order", VariableType = eVariableType.INT, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });

            entityEntity.HasMany(new RelatedEntity() { Name = "PropertyGroup", DisplayName = "Property Group", Entity = propertyGroupEntity, IsRequired = true, IsHidden = true });

            relatedEntityEntity.HasForeignKeyOF(new RelatedEntity() { Name = "Entity", DisplayName = "Entity", Entity = entityEntity, IsVisibleInListingScreen = true, IsRequired = true, CascadeOnDelete = false });

            order = 0;
            Entity projectEntity = application.AddEntity(new Entity() { Name = "Project", DisplayName = "Project", DisplayInDashBoardMenu = true });
            projectEntity.AddProperty(new Property() { Name = "Name", DisplayName = "Name", VariableType = eVariableType.ALPHA_NUMERIC_STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            projectEntity.AddProperty(new Property() { Name = "DisplayName", DisplayName = "Display Name", VariableType = eVariableType.STRING, IsRequired = true, IsVisibleInListingScreen = true, Order = order++ });
            projectEntity.AddProperty(new Property() { Name = "IsDebugMode", DisplayName = "Is Debug Mode", VariableType = eVariableType.BOOL, IsRequired = true, DefaultValue = "false", IsVisibleInListingScreen = true, Order = order++ });
            projectEntity.HasMany(new RelatedEntity() { Name = "Entity", DisplayName = "Entity", Entity = entityEntity, IsRequired = false, IsHidden = true });

            project.Generate();
        }
```
