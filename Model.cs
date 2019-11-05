using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika
{
    public class Model : Base
    {
        public void Create(string modelDirectory, Application application, Entity entity)
        {
            string fileName = System.IO.Path.Combine(modelDirectory, entity.Name + "Model.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using System;");
            f.WriteLine("using System.Collections.Generic;");
            f.WriteLine("using System.ComponentModel.DataAnnotations;");
            f.WriteLine("using System.ComponentModel.DataAnnotations.Schema;");
            f.WriteLine("");
            f.WriteLine("namespace {0}.Models", application.Name);
            f.WriteLine("{");
            f.WriteLine("    [Table(name: \"tbl_{0}\")]", entity.Name);
            f.WriteLine("    public class {0}Model : BaseModel", entity.Name);
            f.WriteLine("    {");

            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("        [ForeignKey(\"{0}\")]", relatedEntity.Name);
                if (relatedEntity.IsRequired)
                    f.WriteLine("        public Guid {0}Id{{ get; set; }}", relatedEntity.Name);
                else
                    f.WriteLine("        public Guid? {0}Id{{ get; set; }}", relatedEntity.Name);
                f.WriteLine("");
                f.WriteLine("        public virtual {0}Model {1}{{ get; set; }}", relatedEntity.Entity.Name, relatedEntity.Name);
                f.WriteLine("");
            }

            foreach (var property in entity.Properties)
            {
                if (property.VariableType == eVariableType.MONEY)
                {
                    f.WriteLine("        [Column(TypeName = \"money\")]");
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Decimal, property.Name);
                }
                else if (property.VariableType == eVariableType.DATEONLY)
                {
                    f.WriteLine("        [Column(TypeName = \"date\")]");
                    f.WriteLine("        public {0}? {1} {{ get; set; }}", VariableType.DateOnly, property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME)
                {
                    f.WriteLine("        public {0}? {1} {{ get; set; }}", VariableType.DateTime, property.Name);
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
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.Email, property.Name);
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
                    f.WriteLine("        [ForeignKey(\"{0}File\")]", property.Name);
                    f.WriteLine("        public {0}? {1}FileId {{ get; set; }}", VariableType.Guid, property.Name);
                    f.WriteLine("        public virtual FileModel {0}File {{ get; set; }}", property.Name);
                }
                else if (property.VariableType == eVariableType.BYTE_ARRAY)
                {
                    f.WriteLine("        public {0} {1} {{ get; set; }}", VariableType.ByteArray, property.Name);
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
            foreach (var relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                f.WriteLine("        public virtual List<{0}Model> {1}s {{ get; set; }} = new List<{0}Model>();", relatedEntity.Entity.Name, relatedEntity.Name);
            }

            f.WriteLine("    }");
            f.WriteLine("}");

            base.Dispose();
        }

        public void CreateBaseModel(string modelDirectory, Application application)
        {
            string fileName = System.IO.Path.Combine(modelDirectory, "BaseModel.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.ComponentModel.DataAnnotations;", "");
            f.WriteLine("using System.ComponentModel.DataAnnotations.Schema;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Models", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public abstract class BaseModel", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        [Key, Column(Order = 0)]", "");
            f.WriteLine("        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]", "");
            f.WriteLine("        public Guid Id {{ get; set; }} = Guid.NewGuid();", "");
            f.WriteLine("", "");
            f.WriteLine("        [Timestamp, ConcurrencyCheck, Column(Order = 1)]", "");
            f.WriteLine("        public byte[] ConcurrencyKey {{ get; set; }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            base.Dispose();
        }

        public void CreateApplicationDbContext(string modelDirectory, Application application)
        {
            string fileName = System.IO.Path.Combine(modelDirectory, "ApplicationDbContext.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.EntityFramework;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Data.Entity;", "");
            f.WriteLine("using System.Data.Entity.Core.Objects;", "");
            f.WriteLine("using System.Data.Entity.Infrastructure;", "");
            f.WriteLine("using System.Data.Entity.ModelConfiguration.Conventions;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Models", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationDbContext()", "");
            f.WriteLine("            : base(\"DefaultConnection\", throwIfV1Schema: false)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            Database.SetInitializer(new ApplicationDbInitializer());", "");
            f.WriteLine("		}}", "");
            f.WriteLine("", "");
            f.WriteLine("        public static ApplicationDbContext Create()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return new ApplicationDbContext();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            foreach (Entity entity in application.Entities)
            {
                f.WriteLine("        public DbSet<{0}Model> {0}s {{ get; set; }}", entity.Name);
            }
            f.WriteLine("", "");
            f.WriteLine("        public override async Task<int> SaveChangesAsync()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return await base.SaveChangesAsync();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("        protected override void OnModelCreating(DbModelBuilder modelBuilder)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            base.OnModelCreating(modelBuilder);", "");
            f.WriteLine("            modelBuilder.Entity<ApplicationUser>().ToTable(\"tbl_User\");", "");
            f.WriteLine("            modelBuilder.Entity<IdentityRole>().ToTable(\"tbl_Role\");", "");
            f.WriteLine("            modelBuilder.Entity<IdentityUserRole>().ToTable(\"tbl_UserRole\");", "");
            f.WriteLine("            modelBuilder.Entity<IdentityUserClaim>().ToTable(\"tbl_UserClaim\");", "");
            f.WriteLine("            modelBuilder.Entity<IdentityUserLogin>().ToTable(\"tbl_UserLogin\");", "");
            foreach (Entity entity in application.Entities)
            {
                foreach (RelatedEntity relatedMultipleEntity in entity.ForeignKeyEntities)
                {
                    if (relatedMultipleEntity.CascadeOnDelete == false && relatedMultipleEntity.IsRequired)
                    {
                        if (relatedMultipleEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Entity == entity) > 0)
                        {
                            if (relatedMultipleEntity.Name == relatedMultipleEntity.Entity.Name)
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany(s => s.{2}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                            else
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany(s => s.{0}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                        else
                        {
                            f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany().HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                    }
                    else if (relatedMultipleEntity.CascadeOnDelete == true && relatedMultipleEntity.IsRequired)
                    {
                        if (relatedMultipleEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Entity == entity) > 0)
                        {
                            if (relatedMultipleEntity.Name == relatedMultipleEntity.Entity.Name)
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany(s => s.{2}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                            else
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany(s => s.{0}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                        else
                        {
                            f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany().HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                    }

                    if (relatedMultipleEntity.CascadeOnDelete == false && !relatedMultipleEntity.IsRequired)
                    {
                        if (relatedMultipleEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Entity == entity) > 0)
                        {
                            if (relatedMultipleEntity.Name == relatedMultipleEntity.Entity.Name)
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany(s => s.{2}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                            else
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany(s => s.{0}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                        else
                        {
                            f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany().HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                    }
                    else if (relatedMultipleEntity.CascadeOnDelete == true && !relatedMultipleEntity.IsRequired)
                    {
                        if (relatedMultipleEntity.Entity.HasManyEntityOfThisTypeList.Count(o => o.Entity == entity) > 0)
                        {
                            if (relatedMultipleEntity.Name == relatedMultipleEntity.Entity.Name)
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany(s => s.{2}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                            else
                                f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany(s => s.{0}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                        else
                        {
                            f.WriteLine("            modelBuilder.Entity<{2}Model>().HasOptional(s => s.{0}).WithMany().HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(true);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                        }
                    }
                }

                //foreach (RelatedEntity relatedMultipleEntity in entity.HasManyEntityOfThisTypeList)
                //{
                //    if (relatedMultipleEntity.CascadeOnDelete == false)
                //    {
                //        f.WriteLine("            modelBuilder.Entity<{2}Model>().HasRequired(s => s.{0}).WithMany(s => s.{2}s).HasForeignKey(s => s.{0}Id).WillCascadeOnDelete(false);", relatedMultipleEntity.Name, relatedMultipleEntity.Entity.Name, entity.Name);
                //    }
                //}
            }
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            base.Dispose();
        }

        public void CreateApplicationDbInitializer(string modelDirectory, Application application)
        {
            string fileName = System.IO.Path.Combine(modelDirectory, "ApplicationDbInitializer.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.EntityFramework;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Data.Entity;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Models", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        protected override void Seed(ApplicationDbContext context)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("			 var store = new RoleStore<IdentityRole>(context);", "");
            f.WriteLine("            var roleManager = new RoleManager<IdentityRole>(store);", "");
            f.WriteLine("            var userStore = new UserStore<ApplicationUser>(context);", "");
            f.WriteLine("            var userManager = new UserManager<ApplicationUser>(userStore);", "");
            f.WriteLine("", "");
            f.WriteLine("            if (!context.Roles.Any(r => r.Name == \"{0}\"))", Jeevika.Properties.Resources.DefaultRole);
            f.WriteLine("            {{", "");
            f.WriteLine("                var role = new IdentityRole {{ Name = \"{0}\" }};", Jeevika.Properties.Resources.DefaultRole);
            f.WriteLine("                roleManager.Create(role);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("            if (!context.Users.Any(u => u.UserName == \"vaduga@outlook.com\"))", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                var user = new ApplicationUser {{ UserName = \"{0}\", EmailConfirmed = true, Email = \"vaduga@outlook.com\" }};", Jeevika.Properties.Resources.DefaultUserName);
            f.WriteLine("                userManager.Create(user, \"{0}\");", Jeevika.Properties.Resources.DefaultPassword);
            f.WriteLine("                userManager.AddToRole(user.Id, \"Admin\");", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("            base.Seed(context);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            base.Dispose();
        }

        public void CreateIdentityModel(string modelDirectory, Application application)
        {
            string fileName = System.IO.Path.Combine(modelDirectory, "IdentityModel.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using System.Data.Entity;", "");
            f.WriteLine("using System.Security.Claims;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.EntityFramework;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Models", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.", "");
            f.WriteLine("    public class ApplicationUser : IdentityUser", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public string FirstName {{ get; set; }}", "");
            f.WriteLine("        public string LastName {{ get; set; }}", "");
            f.WriteLine("        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType", "");
            f.WriteLine("            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);", "");
            f.WriteLine("            // Add custom user claims here", "");
            f.WriteLine("            return userIdentity;", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");


            base.Dispose();
        }
    }
}
