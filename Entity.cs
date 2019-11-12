using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class Entity : Base
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ListingScreenTitle { get; set; }
        public string AddScreenTitle { get; set; }
        public string EditScreenTitle { get; set; }
        public string DetailScreenTitle { get; set; }
        //public bool IsRequired { get; set; } = false;
        //public bool IsNullable { get; set; } = false;
        public bool DisplayInDashBoardMenu { get; set; } = false;
        public bool IsRelationShipTable { get; set; } = false;
        public bool IsViewRequired { get; set; } = true;
        public bool DisplayInAllFiles { get; set; } = false;
        public int Order { get; set; }
        //public bool IsDisplayNameRequired { get; set; } = true;
        public List<RelatedEntity> ForeignKeyEntities { get; set; } = new List<RelatedEntity>();
        public List<RelatedEntity> HasOneEntityOfThisTypeList { get; set; } = new List<RelatedEntity>();
        public List<RelatedEntity> HasManyEntityOfThisTypeList { get; set; } = new List<RelatedEntity>();
        public List<Property> Properties { get; set; } = new List<Property>();
        public List<PropertyGroup> PropertieGroups { get; set; } = new List<PropertyGroup>();

        public Property AddProperty(Property property)
        {
            Properties.Add(property);
            return property;
        }

        public PropertyGroup AddPropertyGroup(PropertyGroup propertyGroup)
        {
            PropertieGroups.Add(propertyGroup);
            return propertyGroup;
        }

        public void HasForeignKeyOF(RelatedEntity relatedEntity)
        {
            ForeignKeyEntities.Add(relatedEntity);

            if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Name == this.Name) > 0)
                return;

            relatedEntity.Entity.HasManyEntityOfThisTypeList.Add(new RelatedEntity() { Name = this.Name, DisplayName = this.DisplayName, Entity = this, ShowInTab = false });
        }

        public void HasMany(RelatedEntity relatedEntity)
        {
            HasManyEntityOfThisTypeList.Add(relatedEntity);
            if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Name == this.Name) == 1)
                return;

            if (relatedEntity.Name != relatedEntity.Entity.Name)
                relatedEntity.Entity.HasForeignKeyOF(new RelatedEntity() { Name = relatedEntity.Name, DisplayName = relatedEntity.DisplayName, Entity = this, CascadeOnDelete = relatedEntity.CascadeOnDelete, IsRequired = relatedEntity.IsRequired, IsVisibleInListingScreen = relatedEntity.IsVisibleInListingScreen, IsHidden = relatedEntity.IsHidden });
            else
                relatedEntity.Entity.HasForeignKeyOF(new RelatedEntity() { Name = this.Name, DisplayName = this.DisplayName, Entity = this, IsRequired = relatedEntity.IsRequired, IsVisibleInListingScreen = relatedEntity.IsVisibleInListingScreen, CascadeOnDelete = relatedEntity.CascadeOnDelete, IsHidden = relatedEntity.IsHidden });
        }

        public void HasOne(RelatedEntity relatedEntity)
        {
            HasOneEntityOfThisTypeList.Add(relatedEntity);
        }
        public void Generate(string applicationDir, Application application)
        {
            var modelDirectory = Path.Combine(applicationDir, "Models");
            base.CreateDirectory(modelDirectory);
            Model model = new Model();
            model.Create(modelDirectory, application, this);

            if (this.IsViewRequired)
            {
                var controllerDirectory = Path.Combine(applicationDir, "Controllers");
                base.CreateDirectory(controllerDirectory);
                Controller controller = new Controller();
                controller.Create(controllerDirectory, application, this);

                var viewDirectory = Path.Combine(applicationDir, "Views");
                base.CreateDirectory(viewDirectory);
                View view = new View();
                view.Create(viewDirectory, application, this);

                var viewModelDirectory = Path.Combine(applicationDir, "ViewModels");
                base.CreateDirectory(viewModelDirectory);
                ViewModel viewModel = new ViewModel();
                viewModel.Create(viewModelDirectory, application, this);
            }
        }


    }
}
