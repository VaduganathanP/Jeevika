using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class ViewModel : Base
    {
        public void Create(string viewModelDir, Application application, Entity entity)
        {
            string fileName = System.IO.Path.Combine(viewModelDir, entity.Name + "ViewModel.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using System;");
            f.WriteLine("using {0}.Converters;", application.Name);
            f.WriteLine("using Newtonsoft.Json;");
            f.WriteLine("using System.Collections.Generic;");
            f.WriteLine("using System.ComponentModel.DataAnnotations;");
            f.WriteLine("using System.Web;");
            f.WriteLine("using System.ComponentModel.DataAnnotations.Schema;");
            f.WriteLine("");
            f.WriteLine("namespace {0}.ViewModels", application.Name);
            f.WriteLine("{");
            f.WriteLine("    public class {0}ViewModel", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("        public Guid Id { get; set; }");
            f.WriteLine("");
            foreach (var property in entity.Properties)
            {
                if (property.IsRequired)
                    f.WriteLine("        [Required]");
                f.WriteLine("        [Display(Name = \"{0}\")]", property.DisplayName);
                if (property.VariableType == eVariableType.MONEY)
                {
                    f.WriteLine("        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = \"{0:c}\")]");
                    f.WriteLine("        [RegularExpression(@\"^\\d+.?\\d{0,2}$\", ErrorMessage = \"Invalid Target Price; Maximum Two Decimal Points.\")]");
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Decimal, property.Name);
                }
                else if (property.VariableType == eVariableType.DATEONLY)
                {
                    f.WriteLine("        [Column(TypeName = \"date\")]", "");
                    f.WriteLine("        public {0}? {1} {{ get; set; }}", VariableType.DateOnly, property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME)
                {
                    f.WriteLine("        public {0}? {1} {{ get; set; }}", VariableType.DateTime, property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME_SECOND)
                {
                    f.WriteLine("        public {0}? {1} {{ get; set; }}", VariableType.DateTimeSecond, property.Name);
                }
                else if (property.VariableType == eVariableType.PHONE)
                {
                    f.WriteLine("        [Phone]");
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Phone, property.Name);
                }
                else if (property.VariableType == eVariableType.EMAIL)
                {
                    f.WriteLine("        [EmailAddress]");
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Email, property.Name);
                }
                else if (property.VariableType == eVariableType.ALPHA_NUMERIC_STRING)
                {
                    f.WriteLine("        [RegularExpression(@\"^[a-zA-Z][a-zA-Z0-9]*$\", ErrorMessage = \"Only Alpha numeric values allowed and should start with an alphabet.\")]");
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.AlphaNumericString, property.Name);
                }
                else if (property.VariableType == eVariableType.INT)
                {
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Int, property.Name);
                }
                else if (property.VariableType == eVariableType.FLOAT)
                {
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Float, property.Name);
                }
                else if (property.VariableType == eVariableType.BOOL)
                {
                    if (!string.IsNullOrEmpty(property.DefaultValue))
                        f.WriteLine("        public {0} {1} {{ get; set; }} = {2};", VariableType.Bool, property.Name, property.DefaultValue);
                    else
                        f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Bool, property.Name);
                }
                else if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("        public {0} {1}Data {{ get; set; }}", VariableType.HttpPostedFileBase, property.Name);
                    f.WriteLine("        public {0}? {1}Id {{ get; set; }}", VariableType.Guid, property.Name);
                    f.WriteLine("        [Display(Name = \"{0}\")]", property.DisplayName);
                    f.WriteLine("        public string {1}Name {{ get; set; }}", VariableType.Guid, property.Name);
                }
                else
                {
                    if (!string.IsNullOrEmpty(property.DefaultValue))
                        f.WriteLine("        public {0} {1} {{ get; set; }} = {2};", VariableType.String, property.Name, property.DefaultValue);
                    else
                        f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.String, property.Name);
                }
                f.WriteLine("");
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("        [Display(Name = \"{0}\")]", relatedEntity.DisplayName);
                if (relatedEntity.IsRequired)
                {
                    f.WriteLine("        [Required]");
                    f.WriteLine("        public Guid {0}Id {{ get; set; }}", relatedEntity.Name);
                }
                else
                {
                    f.WriteLine("        public Guid? {0}Id {{ get; set; }}", relatedEntity.Name);
                }
                f.WriteLine("        public string {0}String {{ get; set; }}", relatedEntity.Name);
            }
            f.WriteLine("    }");
            f.WriteLine("    public class {0}DetailViewModel : {0}ViewModel", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("    }");
            f.WriteLine("    public class {0}AddViewModel : {0}ViewModel", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("    }");
            f.WriteLine("    public class {0}EditViewModel : {0}ViewModel", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("    }");
            f.WriteLine("    public class {0}ListViewModel : {0}ViewModel", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("        public List<{0}ViewModel> {0}List {{ get; set; }} = new List<{0}ViewModel>();", entity.Name);
            f.WriteLine("    }");
            f.WriteLine("}");

            base.Dispose();
        }
    }
}
