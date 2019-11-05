using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class Controller : Base
    {
        public void Create(string controllerDir, Application application, Entity entity)
        {
            string fileName = System.IO.Path.Combine(controllerDir, entity.Name + "Controller.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using {0}.ViewModels;", application.Name);
            f.WriteLine("using System;");
            f.WriteLine("using System.Collections.Generic;");
            f.WriteLine("using System.Linq;");
            f.WriteLine("using System.Threading.Tasks;");
            f.WriteLine("using System.Web;");
            f.WriteLine("using System.IO;");
            f.WriteLine("using System.Web.Mvc;");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{");
            f.WriteLine("    [Authorize]");
            f.WriteLine("    public class {0}Controller : BaseController", entity.Name);
            f.WriteLine("    {");
            f.WriteLine("        public ActionResult Index()");
            f.WriteLine("        {");
            f.WriteLine("            {0}ListViewModel {1}ListViewModel = new {0}ListViewModel();", entity.Name, entity.Name.ToLower());
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                var _tmp1 = String.Format("            var _{0}List = db.{1}s", entity.Name.ToLower(), entity.Name);
                foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
                {
                    if (relatedEntity.IsVisibleInListingScreen)
                    {
                        _tmp1 = _tmp1 + string.Format(".Include(\"{0}\")", relatedEntity.Name);
                    }
                }
                _tmp1 = _tmp1 + ";";
                f.WriteLine(_tmp1);
            }
            else
                f.WriteLine("            var _{0}List = db.{1}s;", entity.Name.ToLower(), entity.Name);
            f.WriteLine("            foreach (var {0} in _{0}List)", entity.Name.ToLower());
            f.WriteLine("            {");
            f.WriteLine("                {0}ListViewModel.{1}List.Add(new {1}ViewModel()", entity.Name.ToLower(), entity.Name);
            f.WriteLine("                {");
            f.WriteLine("                    Id = {0}.Id,", entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                    {0}Id = {1}.{0}FileId,", property.Name, entity.Name.ToLower());
                }
                else
                    f.WriteLine("                    {0} = {1}.{0},", property.Name, entity.Name.ToLower());
            }
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsVisibleInListingScreen)
                {
                    f.WriteLine("                    {0}String = {1}.{0} == null ? string.Empty : {1}.{0}.Name,", relatedEntity.Name, entity.Name.ToLower());
                }
            }
            f.WriteLine("                });");
            f.WriteLine("            }");
            f.WriteLine("            return View({0}ListViewModel);", entity.Name.ToLower());
            f.WriteLine("        }");
            f.WriteLine("");
            f.WriteLine("        [HttpGet]");
            f.WriteLine("        public ActionResult Detail(Guid id, string returnUrl)");
            f.WriteLine("        {");
            f.WriteLine("            ViewBag.ReturnUrl = returnUrl;", "");
            f.WriteLine("            {0}Model {1} = db.{0}s.Find(id);", entity.Name, entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("           FileModel {0}File = null;", property.Name.ToLower());
                    f.WriteLine("           if({0}.{1}FileId != null)", entity.Name.ToLower(), property.Name);
                    f.WriteLine("           {{", "");
                    f.WriteLine("               {2}File = db.Files.Find({0}.{1}FileId);", entity.Name.ToLower(), property.Name, property.Name.ToLower());
                    f.WriteLine("           }}", "");
                }
            }
            f.WriteLine("            {0}DetailViewModel {1}DetailViewModel = new {0}DetailViewModel()", entity.Name, entity.Name.ToLower());
            f.WriteLine("            {");
            f.WriteLine("                Id = {0}.Id,", entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                {0}Id = {1}.{0}FileId,", property.Name, entity.Name.ToLower());
                    f.WriteLine("                {0}Name = {1}File == null ? string.Empty : {1}File.Name,", property.Name, property.Name.ToLower());
                }
                else
                    f.WriteLine("                {0} = {1}.{0},", property.Name, entity.Name.ToLower());
            }
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("                {0}Id = {1}.{0}Id,", relatedEntity.Name, entity.Name.ToLower());
            }
            f.WriteLine("            };");
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("			 List<KeyValuePair<Guid, string>> {0}s = db.{1}s.Select(o => new {{ o.Id, o.Name }}).ToDictionary(o => o.Id, o => o.Name).ToList();", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Entity.Name);
                //f.WriteLine("            {0}s.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, \"--Select--\"));", relatedEntity.Name.ToLowerFirstChar());
                f.WriteLine("            ViewBag.{1}List = new SelectList({0}s, \"Key\", \"Value\");", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Name);
            }
            foreach (RelatedEntity relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("            db.Entry({0}).Collection(s => s.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
                    f.WriteLine("            ViewBag.{0}Count = {1}.{0}s.Count();", relatedEntity.Name, entity.Name.ToLower());
                }
                else
                {
                    if (relatedEntity.Name == relatedEntity.Entity.Name)
                        f.WriteLine("            ViewBag.{2}Count = db.{3}s.Where(o => o.{0}Id == {1}.Id).Count();", entity.Name, entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Entity.Name);
                    else
                        f.WriteLine("            ViewBag.{2}Count = db.{3}s.Where(o => o.{2}Id == {1}.Id).Count();", entity.Name, entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Entity.Name);
                }
            }
            f.WriteLine("            ViewBag.db = db;");
            f.WriteLine("            return View({0}DetailViewModel);", entity.Name.ToLower());
            f.WriteLine("        }");
            f.WriteLine("");
            foreach (RelatedEntity relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                f.WriteLine("        [HttpPost]", "");
                f.WriteLine("        public ActionResult Add{0}s(List<Guid> {1}IdList, Guid {2}Id)", relatedEntity.Name, relatedEntity.Entity.Name.ToLower(), entity.Name.ToLower());
                f.WriteLine("        {{", "");
                f.WriteLine("            {0}Model {1} = db.{0}s.Find({1}Id);", entity.Name, entity.Name.ToLower());
                f.WriteLine("            foreach (Guid id in {0}IdList)", relatedEntity.Entity.Name.ToLower());
                f.WriteLine("                {0}.{2}s.Add(db.{1}s.Find(id));", entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name);
                f.WriteLine("            if ({0}IdList.Count > 0)", relatedEntity.Entity.Name.ToLower());
                f.WriteLine("                db.SaveChanges();", "");
                f.WriteLine("            return Json(true);", "");
                f.WriteLine("        }}", "");
                f.WriteLine("", "");
                f.WriteLine("        [HttpPost]", "");
                f.WriteLine("        public ActionResult Get{0}List(Guid id)", relatedEntity.Name);
                f.WriteLine("        {{", "");
                f.WriteLine("            {0}Model {1} = db.{0}s.Find(id);", entity.Name, entity.Name.ToLower());
                f.WriteLine("            List<{0}ViewModel> {1}ViewModelList = new List<{0}ViewModel>();", relatedEntity.Entity.Name, relatedEntity.Name.ToLower());
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("            db.Entry({0}).Collection(s => s.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
                    f.WriteLine("            foreach (var {0} in {1}.{2}s)", relatedEntity.Name.ToLower(), entity.Name.ToLower(), relatedEntity.Name);
                }
                else
                {
                    if (relatedEntity.Name == relatedEntity.Entity.Name)
                        f.WriteLine("            List<{2}Model> {3}s = db.{2}s.Where(o => o.{0}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                    else
                        f.WriteLine("            List<{2}Model> {3}s = db.{2}s.Where(o => o.{4}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                    f.WriteLine("            foreach (var {0} in {0}s)", relatedEntity.Name.ToLower(), entity.Name.ToLower(), relatedEntity.Entity.Name);
                }

                f.WriteLine("            {{", "");
                f.WriteLine("                {0}ViewModelList.Add(new {1}ViewModel()", relatedEntity.Name.ToLower(), relatedEntity.Entity.Name);
                f.WriteLine("                {{", "");
                f.WriteLine("                    Id = {0}.Id,", relatedEntity.Name.ToLower());
                foreach (var prop in relatedEntity.Entity.Properties)
                {
                    if (prop.IsVisibleInListingScreen)
                    {
                        f.WriteLine("                    {0} = {1}.{0},", prop.Name, relatedEntity.Name.ToLower());
                    }
                }
                foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                {
                    if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                    {
                        f.WriteLine("                    {0}String = {1}.{0} == null ? string.Empty : {1}.{0}.Name,", fke.Name, relatedEntity.Name.ToLower());
                    }
                }
                f.WriteLine("                }});", "");
                f.WriteLine("            }}", "");
                f.WriteLine("", "");
                f.WriteLine("            return Json({0}ViewModelList);", relatedEntity.Name.ToLower());
                f.WriteLine("        }}", "");
                f.WriteLine("", "");
                f.WriteLine("", "");
                f.WriteLine("        [HttpPost]", "");
                f.WriteLine("        public ActionResult GetNotAdded{0}List(Guid id)", relatedEntity.Name);
                f.WriteLine("        {{", "");
                f.WriteLine("            {0}Model {1} = db.{0}s.Find(id);", entity.Name, entity.Name.ToLower());
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("            db.Entry({0}).Collection(s => s.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
                    f.WriteLine("            List<{2}Model> {3}s = {1}.{4}s.ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                }
                else
                {
                    if (relatedEntity.Name == relatedEntity.Entity.Name)
                        f.WriteLine("            List<{2}Model> {3}s = db.{2}s.Where(o => o.{0}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                    else
                        f.WriteLine("            List<{2}Model> {3}s = db.{2}s.Where(o => o.{4}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                }

                f.WriteLine("            List<Guid> {2}{1}IdList = {2}s.Select(s => s.Id).ToList();", entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower());
                f.WriteLine("", "");
                f.WriteLine("            List<{0}Model> {1}List = db.{0}s.Where(o => !{1}{0}IdList.Contains(o.Id)).ToList();", relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), entity.Name.ToLower());
                f.WriteLine("            List<{0}ViewModel> {1}ListViewModels = new List<{0}ViewModel>();", relatedEntity.Entity.Name, relatedEntity.Name.ToLower());
                f.WriteLine("            foreach (var {0} in {0}List)", relatedEntity.Name.ToLower());
                f.WriteLine("            {{", "");
                f.WriteLine("                {0}ListViewModels.Add(new {1}ViewModel()", relatedEntity.Name.ToLower(), relatedEntity.Entity.Name);
                f.WriteLine("                {{", "");
                f.WriteLine("                    Id = {0}.Id,", relatedEntity.Name.ToLower());
                foreach (var prop in relatedEntity.Entity.Properties)
                {
                    if (prop.IsVisibleInListingScreen)
                    {
                        f.WriteLine("                    {1} = {0}.{1},", relatedEntity.Name.ToLower(), prop.Name);
                    }
                }
                foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                {
                    if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                    {
                        f.WriteLine("                    {0}String = {1}.{0} == null ? string.Empty : {1}.{0}.Name,", fke.Name, relatedEntity.Name.ToLower());
                    }
                }
                f.WriteLine("                }});", "");
                f.WriteLine("            }}", "");
                f.WriteLine("", "");
                f.WriteLine("            return Json({0}ListViewModels);", relatedEntity.Name.ToLower());
                f.WriteLine("        }}", "");
                f.WriteLine("", "");
                f.WriteLine("        [HttpPost]", "");
                f.WriteLine("        public ActionResult Remove{0}(Guid {1}Id, Guid {2}Id)", relatedEntity.Name, relatedEntity.Name.ToLower(), entity.Name.ToLower());
                f.WriteLine("        {{", "");
                f.WriteLine("            var {0} = db.{1}s.Find({0}Id);", entity.Name.ToLower(), entity.Name);
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("            db.Entry({0}).Collection(s => s.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
                    f.WriteLine("            List<{2}Model> {3}s = {1}.{4}s.ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                }
                else
                {
                    if (relatedEntity.Name == relatedEntity.Entity.Name)
                        f.WriteLine("            List<{2}Model> {5}s = db.{2}s.Where(o => o.{0}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Name.ToLower());
                    else
                        f.WriteLine("            List<{2}Model> {5}s = db.{2}s.Where(o => o.{4}Id == {1}.Id).ToList();", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Name.ToLower());
                }
                f.WriteLine("            db.Entry({0}).Collection(o => o.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
                if (entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == relatedEntity.Entity).Count() > 0 && relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("            {0}.{1}s.Remove({2}s.FirstOrDefault(o => o.Id == {2}Id));", entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Entity.Name.ToLower());
                }
                else
                {
                    f.WriteLine("            db.{3}s.Remove({2}s.FirstOrDefault(o => o.Id == {2}Id));", entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Entity.Name.ToLower());
                }
                f.WriteLine("            if (db.SaveChanges() != 0)", "");
                f.WriteLine("                return Json(true);", "");
                f.WriteLine("            else", "");
                f.WriteLine("                return Json(false);", "");
                f.WriteLine("        }}", "");
            }
            f.WriteLine("");
            string fkParms = string.Empty;
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
                fkParms += "Guid? " + relatedEntity.Name + "Id,";
            if (fkParms != string.Empty)
                fkParms = fkParms.Substring(0, fkParms.Length - 1) + ", string returnUrl";
            else
                fkParms = "string returnUrl";
            f.WriteLine("        [HttpGet]");
            f.WriteLine("        public ActionResult Add({0})", fkParms);
            f.WriteLine("        {");
            f.WriteLine("            ViewBag.ReturnUrl = returnUrl;", "");
            f.WriteLine("            {0}AddViewModel model = new {0}AddViewModel();", entity.Name);
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("            if({0} != null)", relatedEntity.Name + "Id");
                f.WriteLine("            {{", "");
                f.WriteLine("                model.{0} = {0}.Value;", relatedEntity.Name + "Id");
                f.WriteLine("            }}", "");
                f.WriteLine("			 List<KeyValuePair<Guid, string>> {0}s = db.{1}s.Select(o => new {{ o.Id, o.Name }}).ToDictionary(o => o.Id, o => o.Name).ToList();", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Entity.Name);
                //f.WriteLine("            {0}s.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, \"--Select--\"));", relatedEntity.Name.ToLowerFirstChar());
                f.WriteLine("            ViewBag.{1}List = new SelectList({0}s, \"Key\", \"Value\");", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Name);
            }
            f.WriteLine("            return View(model);");
            f.WriteLine("        }");
            f.WriteLine("");
            f.WriteLine("        [HttpPost]");
            f.WriteLine("        [ValidateAntiForgeryToken]");
            f.WriteLine("        public async Task<ActionResult> Add({0}AddViewModel model, string returnUrl)", entity.Name);
            f.WriteLine("        {");
            f.WriteLine("            if (!ModelState.IsValid)");
            f.WriteLine("            {");
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("               Dictionary<Guid, string> {0}s = db.{1}s.Select(o => new {{ o.Id, o.Name }}).ToDictionary(o => o.Id, o => o.Name);", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Entity.Name);
                //f.WriteLine("               {0}s.Add(Guid.Empty, \"--Select--\");", relatedEntity.Name.ToLowerFirstChar());
                f.WriteLine("               ViewBag.{1}List = new SelectList({0}s, \"Key\", \"Value\");", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Name);
            }
            f.WriteLine("               return View(model);");
            f.WriteLine("            }");
            List<Property> fileTypeProperties = entity.Properties.Where(o => o.VariableType == eVariableType.FILE).ToList();
            foreach (var fileTypeProperty in fileTypeProperties)
            {
                f.WriteLine("            Guid? {0}Id = null;", fileTypeProperty.Name.ToLower());
                f.WriteLine("            if (model.{0}Data != null)", fileTypeProperty.Name);
                f.WriteLine("            {{", "");
                f.WriteLine("                byte[] {0}Data = null;", fileTypeProperty.Name.ToLower());
                f.WriteLine("                using (var binaryReader = new BinaryReader(model.{0}Data.InputStream))", fileTypeProperty.Name);
                f.WriteLine("                {{", "");
                f.WriteLine("                    {1}Data = binaryReader.ReadBytes(model.{0}Data.ContentLength);", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                }}", "");
                f.WriteLine("", "");
                f.WriteLine("                FileModel {0}File = new FileModel();", fileTypeProperty.Name.ToLower());
                f.WriteLine("                {1}File.Name = model.{0}Data.FileName;", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                {1}File.ContentType = model.{0}Data.ContentType;", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                {1}File.ContentLength = model.{0}Data.ContentLength;", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                {1}File.Data = {1}Data;", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                db.Files.Add({1}File);", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("                {1}Id = {1}File.Id;", fileTypeProperty.Name, fileTypeProperty.Name.ToLower());
                f.WriteLine("            }}", "");
            }
            f.WriteLine("            {0}Model {1} = new {0}Model()", entity.Name, entity.Name.ToLower());
            f.WriteLine("            {");
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                {0}FileId = {1}Id,", property.Name, property.Name.ToLower());
                }
                else
                    f.WriteLine("                {0} = model.{0},", property.Name);
            }
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("                {0}Id = model.{0}Id,", relatedEntity.Name);
            }
            f.WriteLine("            };");
            f.WriteLine("            db.{0}s.Add({1});", entity.Name, entity.Name.ToLower());
            f.WriteLine("            await db.SaveChangesAsync();");
            f.WriteLine("            if(returnUrl != null)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return Redirect(returnUrl);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            return RedirectToAction(\"Index\");");
            f.WriteLine("        }");
            f.WriteLine("");
            f.WriteLine("        [HttpGet]");
            f.WriteLine("        public ActionResult Edit(Guid id, string returnUrl)");
            f.WriteLine("        {");
            f.WriteLine("            ViewBag.ReturnUrl = returnUrl;", "");
            f.WriteLine("            {0}Model {1} = db.{0}s.Find(id);", entity.Name, entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("           FileModel {0}File = null;", property.Name.ToLower());
                    f.WriteLine("           if({0}.{1}FileId != null)", entity.Name.ToLower(), property.Name);
                    f.WriteLine("           {{", "");
                    f.WriteLine("               {2}File = db.Files.Find({0}.{1}FileId);", entity.Name.ToLower(), property.Name, property.Name.ToLower());
                    f.WriteLine("           }}", "");
                }
            }
            f.WriteLine("            {0}EditViewModel {1}EditViewModel = new {0}EditViewModel()", entity.Name, entity.Name.ToLower());
            f.WriteLine("            {");
            f.WriteLine("                Id = {0}.Id,", entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                {0}Id = {1}.{0}FileId,", property.Name, entity.Name.ToLower());
                    f.WriteLine("                {0}Name = {1}File == null ? string.Empty : {1}File.Name,", property.Name, property.Name.ToLower());
                }
                else
                    f.WriteLine("                {0} = {1}.{0},", property.Name, entity.Name.ToLower());
            }
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("                {0}Id = {1}.{0}Id,", relatedEntity.Name, entity.Name.ToLower());
            }
            f.WriteLine("            };");
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("			 List<KeyValuePair<Guid, string>> {0}s = db.{1}s.Select(o => new {{ o.Id, o.Name }}).ToDictionary(o => o.Id, o => o.Name).ToList();", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Entity.Name);
                //f.WriteLine("            {0}s.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, \"--Select--\"));", relatedEntity.Name.ToLowerFirstChar());
                f.WriteLine("            ViewBag.{1}List = new SelectList({0}s, \"Key\", \"Value\");", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Name);
            }
            f.WriteLine("            return View({0}EditViewModel);", entity.Name.ToLower());
            f.WriteLine("        }");
            f.WriteLine("");
            f.WriteLine("        [HttpPost]");
            f.WriteLine("        [ValidateAntiForgeryToken]");
            f.WriteLine("        public async Task<ActionResult> Edit({0}EditViewModel model, string returnUrl)", entity.Name);
            f.WriteLine("        {");
            f.WriteLine("            if (!ModelState.IsValid)");
            f.WriteLine("            {");
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("			 List<KeyValuePair<Guid, string>> {0}s = db.{1}s.Select(o => new {{ o.Id, o.Name }}).ToDictionary(o => o.Id, o => o.Name).ToList();", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Entity.Name);
                //f.WriteLine("            {0}s.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, \"--Select--\"));", relatedEntity.Name.ToLowerFirstChar());
                f.WriteLine("            ViewBag.{1}List = new SelectList({0}s, \"Key\", \"Value\");", relatedEntity.Name.ToLowerFirstChar(), relatedEntity.Name);
            }
            f.WriteLine("                return View(model);");
            f.WriteLine("            }");
            f.WriteLine("            {0}Model {1} = await db.{0}s.FindAsync(model.Id);", entity.Name, entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("            Guid? {0}Id = null;", property.Name.ToLower());
                    f.WriteLine("            if ({0}.{1}FileId != null)", entity.Name.ToLower(), property.Name);
                    f.WriteLine("            {{", "");
                    f.WriteLine("                FileModel {0}File = db.Files.Find({1}.{2}FileId);", property.Name.ToLower(), entity.Name.ToLower(), property.Name);
                    f.WriteLine("                if (model.{0}Id == null || {1}.{0}FileId != model.{0}Id)", property.Name, entity.Name.ToLower());
                    f.WriteLine("                {{", "");
                    f.WriteLine("                    db.Files.Remove({0}File);", property.Name.ToLower());
                    f.WriteLine("                    if (model.{0}Data != null)", property.Name);
                    f.WriteLine("                    {{", "");
                    f.WriteLine("                        byte[] {0}Data = null;", property.Name.ToLower());
                    f.WriteLine("                        using (var binaryReader = new BinaryReader(model.{0}Data.InputStream))", property.Name);
                    f.WriteLine("                        {{", "");
                    f.WriteLine("                            {0}Data = binaryReader.ReadBytes(model.{1}Data.ContentLength);", property.Name.ToLower(), property.Name);
                    f.WriteLine("                        }}", "");
                    f.WriteLine("", "");
                    f.WriteLine("                        FileModel new{0}File = new FileModel();", property.Name);
                    f.WriteLine("                        new{0}File.Name = model.{0}Data.FileName;", property.Name);
                    f.WriteLine("                        new{0}File.ContentType = model.{0}Data.ContentType;", property.Name);
                    f.WriteLine("                        new{0}File.ContentLength = model.{0}Data.ContentLength;", property.Name);
                    f.WriteLine("                        new{0}File.Data = {1}Data;", property.Name, property.Name.ToLower());
                    f.WriteLine("                        db.Files.Add(new{0}File);", property.Name);
                    f.WriteLine("                        {0}Id = new{1}File.Id;", property.Name.ToLower(), property.Name);
                    f.WriteLine("                    }}", "");
                    f.WriteLine("                }}", "");
                    f.WriteLine("            }}", "");
                    f.WriteLine("            if ({0}.{1}FileId == model.{1}Id && {0}.{1}FileId != null)", entity.Name.ToLower(), property.Name);
                    f.WriteLine("            {{", "");
                    f.WriteLine("                {0}Id = model.{1}Id;", property.Name.ToLower(), property.Name);
                    f.WriteLine("            }}", "");
                    //f.WriteLine("            if ({0}.{1}FileId == null)", entity.Name.ToLower(), property.Name);
                    //f.WriteLine("            {{", "");
                    //f.WriteLine("                if(model.{0}Data != null)", property.Name);
                    //f.WriteLine("                {{", "");
                    //f.WriteLine("                   byte[] {0}Data = null;", property.Name.ToLower());
                    //f.WriteLine("                   using (var binaryReader = new BinaryReader(model.{0}Data.InputStream))", property.Name);
                    //f.WriteLine("                   {{", "");
                    //f.WriteLine("                       {0}Data = binaryReader.ReadBytes(model.{1}Data.ContentLength);", property.Name.ToLower(), property.Name);
                    //f.WriteLine("                   }}", "");
                    //f.WriteLine("", "");
                    //f.WriteLine("                   FileModel new{0}File = new FileModel();", property.Name);
                    //f.WriteLine("                   new{0}File.Name = model.{0}Data.FileName;", property.Name);
                    //f.WriteLine("                   new{0}File.ContentType = model.{0}Data.ContentType;", property.Name);
                    //f.WriteLine("                   new{0}File.ContentLength = model.{0}Data.ContentLength;", property.Name);
                    //f.WriteLine("                   new{0}File.Data = {1}Data;", property.Name, property.Name.ToLower());
                    //f.WriteLine("                   db.Files.Add(new{0}File);", property.Name);
                    //f.WriteLine("                   {0}Id = new{1}File.Id;", property.Name.ToLower(), property.Name);
                    //f.WriteLine("                }}", "");
                    //f.WriteLine("            }}", "");

                    f.WriteLine("            {0}.{1}FileId = {2}Id;", entity.Name.ToLower(), property.Name, property.Name.ToLower());
                }
                else
                    f.WriteLine("            {0}.{1} = model.{1};", entity.Name.ToLower(), property.Name);
            }
            foreach (RelatedEntity relatedEntity in entity.ForeignKeyEntities)
            {
                f.WriteLine("            {0}.{1}Id = model.{1}Id;", entity.Name.ToLower(), relatedEntity.Name);
            }
            f.WriteLine("            await db.SaveChangesAsync();");
            f.WriteLine("            if(returnUrl != null)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return Redirect(returnUrl);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            return RedirectToAction(\"Index\");");
            f.WriteLine("        }");
            f.WriteLine("");
            f.WriteLine("        [HttpPost]");
            f.WriteLine("        public async Task<ActionResult> Delete(Guid id)");
            f.WriteLine("        {");
            f.WriteLine("            var {0} = await db.{1}s.FindAsync(id);", entity.Name.ToLower(), entity.Name);
            foreach (RelatedEntity relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                f.WriteLine("            db.Entry({0}).Collection(o => o.{1}s).Load();", entity.Name.ToLower(), relatedEntity.Name);
            }
            f.WriteLine("            if ({0} != null)", entity.Name.ToLower());
            f.WriteLine("            {");
            f.WriteLine("                db.{0}s.Remove({1});", entity.Name, entity.Name.ToLower());
            f.WriteLine("                await db.SaveChangesAsync();");
            f.WriteLine("                return Json(true);");
            f.WriteLine("            }");
            f.WriteLine("            else");
            f.WriteLine("            {");
            f.WriteLine("                return Json(false);");
            f.WriteLine("            }");
            f.WriteLine("        }");
            f.WriteLine("    }");
            f.WriteLine("}");

            base.Dispose();
        }

        public void CreateBaseController(string controllerDir, Application application)
        {
            string fileName = System.IO.Path.Combine(controllerDir, "BaseController.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public abstract class BaseController : Controller", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public BaseController()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            db = ApplicationDbContext.Create();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("        protected ApplicationDbContext db {{ get; private set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        protected override void Dispose(bool disposing)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            if (disposing)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                db.Dispose();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            base.Dispose(disposing);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            base.Dispose();
        }

        public void CreateFileController(string controllerDir, Application application)
        {
            string fileName = System.IO.Path.Combine(controllerDir, "FileController.cs");
            StreamWriter f = base.CreateFile(fileName);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    [Authorize]", "");
            f.WriteLine("    public class FileController : BaseController", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        [HttpGet]", "");
            f.WriteLine("        public FileResult DownLoadFile(Guid id)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            FileModel file = db.Files.Find(id);", "");
            f.WriteLine("            return File(file.Data, file.ContentType, file.Name);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            base.Dispose();
        }
    }
}
