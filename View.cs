using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class View : Base
    {
        public void Create(string viewDir, Application application, Entity entity)
        {
            string modelDir = System.IO.Path.Combine(viewDir, entity.Name);
            if (!Directory.Exists(modelDir))
                Directory.CreateDirectory(modelDir);

            string indexFileName = System.IO.Path.Combine(modelDir, "Index.cshtml");
            StreamWriter indexFile = base.CreateFile(indexFileName);
            CreateIndexFile(indexFile, application, entity);

            string addFileName = System.IO.Path.Combine(modelDir, "Add.cshtml");
            StreamWriter addFile = base.CreateFile(addFileName);
            CreateAddFile(addFile, application, entity);

            string editFileName = System.IO.Path.Combine(modelDir, "Edit.cshtml");
            StreamWriter editFile = base.CreateFile(editFileName);
            CreateEditFile(editFile, application, entity);

            string detailFileName = System.IO.Path.Combine(modelDir, "Detail.cshtml");
            StreamWriter detailFile = base.CreateFile(detailFileName);
            CreateDetailFile(detailFile, application, entity);

            base.Dispose();
        }

        private void BreadCrumbObjectGenerator(Entity entity, ref List<Entity> entities)
        {
            if (entity.ForeignKeyEntities.Count > 0)
            {
                foreach (var relatedEntity in entity.ForeignKeyEntities)
                {
                    if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() == 1 && relatedEntity.Entity.HasManyEntityOfThisTypeList.Count( o => o.Name == entity.Name) == 0)
                    {
                        entities.Add(relatedEntity.Entity);
                        BreadCrumbObjectGenerator(relatedEntity.Entity, ref entities);
                    }
                }
            }
        }
        private void CreateIndexFile(StreamWriter f, Application application, Entity entity)
        {
            f.WriteLine("@model {0}.ViewModels.{1}ListViewModel", application.Name, entity.Name);
            f.WriteLine("", "");
            f.WriteLine("@{{", "");
            f.WriteLine("    ViewBag.Title = \"{0} List\";", entity.DisplayName);
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("@section styles{{", "");
            f.WriteLine("    <link href=\"//cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    <style>", "");
            f.WriteLine("        table {{", "");
            f.WriteLine("            visibility: hidden;", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    </style>", "");
            f.WriteLine("}}", "");
            f.WriteLine("<h1 class=\"h3 mb-2 text-gray-800\">{0}</h1>", entity.DisplayName);
            f.WriteLine("<p class=\"mb-4\">{0}</p>", entity.ListingScreenTitle);
            f.WriteLine("", "");
            f.WriteLine("<div class=\"card shadow mb-4\">", "");
            f.WriteLine("    <div class=\"card-header py-3 d-flex flex-row align-items-center justify-content-between\">", "");
            f.WriteLine("        <h6 class=\"m-0 font-weight-bold text-primary\">{0} List</h6>", entity.DisplayName);
            f.WriteLine("        <a href=\"@Url.Action(\"Add\",\"{0}\", new {{ @returnUrl = Request.Url.AbsoluteUri }})\" class=\"btn btn-success btn-icon-split btn-sm\">", entity.Name);
            f.WriteLine("            <span class=\"icon text-white-50\">", "");
            f.WriteLine("                <i class=\"fas fa-plus\"></i>", "");
            f.WriteLine("            </span>", "");
            f.WriteLine("            <span class=\"text\">Add New</span>", "");
            f.WriteLine("        </a>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("    <div class=\"card-body\">", "");
            f.WriteLine("        <div class=\"table-responsive\">", "");
            f.WriteLine("            <table class=\"table table-bordered\" id=\"tbl{0}\" width=\"100%\" cellspacing=\"0\">", entity.Name);
            f.WriteLine("                <thead>", "");
            f.WriteLine("                    <tr>", "");
            f.WriteLine("                        <th>Id</th>", "");
            foreach (Property property in entity.Properties)
            {
                if (property.IsVisibleInListingScreen)
                    f.WriteLine("                        <th>{0}</th>", property.DisplayName);
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsVisibleInListingScreen)
                    f.WriteLine("                        <th>{0}</th>", relatedEntity.DisplayName);
            }

            f.WriteLine("                        <th>Command</th>", "");
            f.WriteLine("                    </tr>", "");
            f.WriteLine("                </thead>", "");
            f.WriteLine("                <tbody>", "");
            f.WriteLine("                    @foreach (var {0} in Model.{1}List)", entity.Name.ToLower(), entity.Name);
            f.WriteLine("                    {{", "");
            f.WriteLine("                    <tr>", "");
            f.WriteLine("                        <td>@{0}.Id</td>", entity.Name.ToLower());
            foreach (Property property in entity.Properties)
            {
                if (property.IsVisibleInListingScreen)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("                        @if ({0}.{1}.HasValue)", entity.Name.ToLower(), property.Name);
                        f.WriteLine("                        {{", "");
                        f.WriteLine("                            <td>@{0}.{1}.Value.ToShortDateString()</td>", entity.Name.ToLower(), property.Name);
                        f.WriteLine("                        }}", "");
                        f.WriteLine("                        else", "");
                        f.WriteLine("                        {{", "");
                        f.WriteLine("                            <td></td>", "");
                        f.WriteLine("                        }}", "");

                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("                        @if ({0}.{1}.HasValue)", entity.Name.ToLower(), property.Name);
                        f.WriteLine("                        {{", "");
                        f.WriteLine("                            <td>@{0}.{1}.Value.ToString(\"MM/dd/yyyy hh:mm:ss\")</td>", entity.Name.ToLower(), property.Name);
                        f.WriteLine("                        }}", "");
                        f.WriteLine("                        else", "");
                        f.WriteLine("                        {{", "");
                        f.WriteLine("                            <td></td>", "");
                        f.WriteLine("                        }}", "");

                    }
                    else if (property.VariableType == eVariableType.MONEY)
                        f.WriteLine("                        <td>@{0}.{1}.ToString(\"0.00\")</td>", entity.Name.ToLower(), property.Name);
                    else
                        f.WriteLine("                        <td>@{0}.{1}</td>", entity.Name.ToLower(), property.Name);
                }

            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsVisibleInListingScreen)
                    f.WriteLine("                        <td>@{0}.{1}String</td>", entity.Name.ToLower(), relatedEntity.Name);
            }
            f.WriteLine("                        <td></td>", "");
            f.WriteLine("                    </tr>", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("", "");
            f.WriteLine("                </tbody>", "");
            f.WriteLine("                <tfoot>", "");
            f.WriteLine("                    <tr>", "");
            f.WriteLine("                        <th>Id</th>", "");
            foreach (Property property in entity.Properties)
            {
                if (property.IsVisibleInListingScreen)
                    f.WriteLine("                        <th>{0}</th>", property.DisplayName);
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsVisibleInListingScreen)
                    f.WriteLine("                        <th>{0}</th>", relatedEntity.DisplayName);
            }
            f.WriteLine("                        <th>Command</th>", "");
            f.WriteLine("                    </tr>", "");
            f.WriteLine("                </tfoot>", "");
            f.WriteLine("            </table>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("</div>", "");
            f.WriteLine("", "");
            f.WriteLine("@section scripts{{", "");
            f.WriteLine("", "");
            f.WriteLine("    <script src=\"//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js\"></script>", "");
            f.WriteLine("", "");
            f.WriteLine("    <script>", "");
            f.WriteLine("        $(document).ready(function () {{", "");
            f.WriteLine("            var table = $('#tbl{0}').DataTable({{", entity.Name);
            f.WriteLine("                \"columnDefs\": [", "");
            f.WriteLine("                    {{", "");
            f.WriteLine("                        \"targets\": [0],", "");
            f.WriteLine("                        \"visible\": false,", "");
            f.WriteLine("                        \"searchable\": false", "");
            f.WriteLine("                    }},", "");
            f.WriteLine("                    {{", "");
            f.WriteLine("                        \"targets\": -1,", "");
            f.WriteLine("                        \"width\": \"100px\",", "");
            f.WriteLine("                        \"className\": \"text-center\",", "");
            f.WriteLine("                        \"data\": null,", "");
            f.WriteLine("                        \"orderable\": false,", "");
            f.WriteLine("                        \"defaultContent\": `<button data-value=\"detail\" class=\"btn btn-primary btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Detail!\">", "");
            f.WriteLine("                                                            <i class=\"fa fa-info\" aria-hidden=\"true\"></i>", "");
            f.WriteLine("                                                       </button>&nbsp;", "");
            f.WriteLine("                                                       <button data-value=\"edit\" class=\"btn btn-warning btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Edit!\">", "");
            f.WriteLine("                                                            <i class=\"fa fa-edit\" aria-hidden=\"true\"></i>", "");
            f.WriteLine("                                                        </button>&nbsp;", "");
            f.WriteLine("                                                       <button data-value=\"delete\" class=\"btn btn-danger btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Delete!\">", "");
            f.WriteLine("                                                            <i class=\"fa fa-trash\" aria-hidden=\"true\"></i>", "");
            f.WriteLine("                                                        </button>`", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("                ]", "");
            f.WriteLine("            }});", "");
            f.WriteLine("            document.getElementsByTagName(\"table\")[0].style.visibility = \"visible\";", "");
            f.WriteLine("", "");
            f.WriteLine("            $('#tbl{0} tbody').on('dblclick', 'tr', function () {{", entity.Name);
            f.WriteLine("                var data = table.row($(this)).data();", "");
            f.WriteLine("                window.location.href = \"@Url.Action(\"Detail\",\"{0}\", new {{ id = \"\" }})/\" + data[0];", entity.Name);
            f.WriteLine("            }});", "");
            f.WriteLine("", "");
            f.WriteLine("            $('#tbl{0} tbody').on('click', 'button', function () {{", entity.Name);
            f.WriteLine("                var action = this.getAttribute(\"data-value\");", "");
            f.WriteLine("                var data = table.row($(this).parents('tr')).data();", "");
            f.WriteLine("", "");
            f.WriteLine("                if (action == 'detail') {{", "");
            f.WriteLine("                    window.location.href = \"@Url.Action(\"Detail\",\"{0}\")/\" + data[0];", entity.Name);
            f.WriteLine("                }}", "");
            f.WriteLine("", "");
            f.WriteLine("                if (action == 'edit') {{", "");
            f.WriteLine("                    window.location.href = \"@Url.Action(\"Edit\",\"{0}\")/\" + data[0] + \"?returnurl=\" + window.location.href;", entity.Name);
            f.WriteLine("                }}", "");
            f.WriteLine("", "");
            f.WriteLine("                if (action == 'delete') {{", "");
            f.WriteLine("                    if (!confirm(\"Are you sure you wish to delete?\")) {{", "");
            f.WriteLine("                        return;", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("                    var row = table.row($(this).parents('tr'));", "");
            f.WriteLine("                    var request = window.location.href;", "");
            f.WriteLine("                    var jqxhr = $.post(request + \"/Delete\", {{ Id: data[0] }}, function (result) {{", "");
            f.WriteLine("                        row.remove().draw();", "");
            f.WriteLine("                    }})", "");
            f.WriteLine("                        .fail(function () {{", "");
            f.WriteLine("                            alert(\"Error updating data, please try again after sometimes\");", "");
            f.WriteLine("                        }})", "");
            f.WriteLine("                }}", "");
            f.WriteLine("", "");
            f.WriteLine("            }});", "");
            f.WriteLine("", "");
            f.WriteLine("        }});", "");
            f.WriteLine("    </script>", "");
            f.WriteLine("}}", "");
        }
        private void CreateAddFile(StreamWriter f, Application application, Entity entity)
        {
            f.WriteLine("@model {0}.ViewModels.{1}AddViewModel", application.Name, entity.Name);
            f.WriteLine("@{{", "");
            f.WriteLine("    ViewBag.Title = \"Add New {0}\";", entity.DisplayName);
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("@section styles{{", "");
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.min.css\"", "");
                f.WriteLine("          integrity=\"sha256-DOS9W6NR+NFe1fUhEE0PGKY/fubbUCnOfTje2JMDw3Y=\" crossorigin=\"anonymous\" />", "");
            }
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/css/bootstrap-select.min.css\" rel=\"stylesheet\" />", "");
            }
            f.WriteLine("", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<nav aria-label=\"breadcrumb\">", "");
            f.WriteLine("    <ol class=\"breadcrumb shadow p-3 mb-3 bg-white rounded\">", "");
            f.WriteLine("        <li class=\"breadcrumb-item\"><a href=\"@Url.Action(\"Index\",\"{0}\")\">{1} List</a></li>", entity.Name, entity.DisplayName);
            f.WriteLine("        <li class=\"breadcrumb-item active\" aria-current=\"page\">Add {0}</li>", entity.DisplayName);
            f.WriteLine("    </ol>", "");
            f.WriteLine("</nav>", "");
            f.WriteLine("", "");
            f.WriteLine("<div class=\"card border-0 shadow-lg my-1\">", "");
            f.WriteLine("", "");
            f.WriteLine("    <div class=\"card-body p-0\">", "");
            f.WriteLine("        <!-- Nested Row within Card Body -->", "");
            f.WriteLine("        <div class=\"row\">", "");
            f.WriteLine("", "");
            f.WriteLine("            <div class=\"col-lg-12\">", "");
            f.WriteLine("                <div class=\"p-5\">", "");
            f.WriteLine("                    <div class=\"text-center\">", "");
            f.WriteLine("                        <h1 class=\"h4 text-gray-900 mb-4\"><u>{0}</u></h1>", entity.AddScreenTitle);
            f.WriteLine("                    </div>", "");
            if (entity.Properties.Where(o => o.VariableType == eVariableType.FILE).Count() > 0)
            {
                f.WriteLine("                    @using (Html.BeginForm(\"Add\", \"{0}\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\", enctype = \"multipart/form-data\" }}))", entity.Name);
            }
            else
            {
                f.WriteLine("                    @using (Html.BeginForm(\"Add\", \"{0}\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\" }}))", entity.Name);
            }
            f.WriteLine("                    {{", "");
            f.WriteLine("                        @Html.AntiForgeryToken()", "");
            f.WriteLine("                        <div class=\"form-row\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType == eVariableType.BOOL)
                    continue;

                if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                            <div class=\"form-group col-md-12\">", "");
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                }

                if (property.IsRequired)
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}</label>", property.Name, property.DisplayName);

                if (property.VariableType == eVariableType.STRING)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.PHONE)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                                @Html.TextAreaFor(m => m.{0}, new {{ @class = \"form-control\", @style = \"height:150px\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATEONLY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy}}\", new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy hh:mm:ss}}\", new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.INT)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @type = \"number\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.MONEY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:0.00}}\", new {{ @class = \"form-control\", @type = \"number\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.BOOL)
                {
                    f.WriteLine("                                <div class=\"form-check\">", property.Name);
                    f.WriteLine("                                   @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"form-check-input\" }})", property.Name);
                    f.WriteLine("                                   @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                    f.WriteLine("                                </div>", property.Name);
                }
                else if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}Data, new {{ @class = \"form-control\", @type = \"file\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}Data\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                f.WriteLine("                            </div>", "");
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsHidden)
                {
                    f.WriteLine("                            @Html.HiddenFor(m => m.{0}Id)", relatedEntity.Name);
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                    if (relatedEntity.IsRequired)
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", relatedEntity.Name, relatedEntity.DisplayName);
                    else
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}</label>", relatedEntity.Name, relatedEntity.DisplayName);

                    f.WriteLine("                                @Html.DropDownList(\"{0}Id\", (SelectList)ViewBag.{0}List, \"--Select--\", new {{ @class = \"form-control selectpicker\", @data_live_search = \"true\" }})", relatedEntity.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}Id\", new {{ @class = \"text-danger\" }})", relatedEntity.Name);
                    f.WriteLine("                            </div>", "");
                }
            }
            f.WriteLine("                        </div>", "");

            if (entity.Properties.Count(o => o.VariableType == eVariableType.BOOL) > 0)
                f.WriteLine("                        <hr class=\"mb-4\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType != eVariableType.BOOL)
                    continue;
                f.WriteLine("                        <div class=\"form-row\">", "");
                f.WriteLine("                            <div class=\"custom-control custom-checkbox\">", "");
                f.WriteLine("                                @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"custom-control-input\" }})", property.Name);
                if (property.IsRequired)
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}</label>", property.Name, property.DisplayName);
                f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                f.WriteLine("                            </div>", "");
                f.WriteLine("                        </div>", "");
            }
            f.WriteLine("                        <hr class=\"mb-4\">", "");
            f.WriteLine("                        <button type=\"submit\" class=\"btn btn-primary\">Add</button>", "");
            f.WriteLine("                        <a class=\"btn btn-primary\" href=\"@ViewBag.ReturnUrl\">Cancel</a>", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("</div>", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("@section scripts{{", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/js/bootstrap-select.min.js\"></script>", "");
            }
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.full.min.js\"", "");
                f.WriteLine("            integrity=\"sha256-FEqEelWI3WouFOo2VWP/uJfs1y8KJ++FLh2Lbqc8SJk=\" crossorigin=\"anonymous\"></script>", "");
                f.WriteLine("    <script type=\"text/javascript\">", "");
                f.WriteLine("", "");
                f.WriteLine("", "");
                f.WriteLine("        $(function () {{", "");
                foreach (var property in entity.Properties)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y',", "");
                        f.WriteLine("                timepicker: false,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y h:m:s',", "");
                        f.WriteLine("                timepicker: true,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                }
                f.WriteLine("        }});", "");
                f.WriteLine("    </script>", "");
            }
            f.WriteLine("}}", "");
        }
        private void CreateEditFile(StreamWriter f, Application application, Entity entity)
        {
            f.WriteLine("@model {0}.ViewModels.{1}EditViewModel", application.Name, entity.Name);
            f.WriteLine("@{{", "");
            f.WriteLine("    ViewBag.Title = \"Edit {0}\";", entity.DisplayName);
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("@section styles{{", "");
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.min.css\"", "");
                f.WriteLine("          integrity=\"sha256-DOS9W6NR+NFe1fUhEE0PGKY/fubbUCnOfTje2JMDw3Y=\" crossorigin=\"anonymous\" />", "");
            }
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/css/bootstrap-select.min.css\" rel=\"stylesheet\" />", "");
            }
            f.WriteLine("", "");
            if (entity.Properties.Where(o => o.VariableType == eVariableType.FILE).Count() > 0)
            {
                f.WriteLine("    <style>", "");
                f.WriteLine("        .custom-file-uploader {{", "");
                f.WriteLine("            position: relative;", "");
                f.WriteLine("        }}", "");
                f.WriteLine("", "");
                f.WriteLine("            .custom-file-uploader input[type='file'] {{", "");
                f.WriteLine("                display: block;", "");
                f.WriteLine("                position: absolute;", "");
                f.WriteLine("                top: 0;", "");
                f.WriteLine("                right: 0;", "");
                f.WriteLine("                bottom: 0;", "");
                f.WriteLine("                left: 0;", "");
                f.WriteLine("                z-index: 5;", "");
                f.WriteLine("                width: 100%;", "");
                f.WriteLine("                height: 100%;", "");
                f.WriteLine("                opacity: 0;", "");
                f.WriteLine("                cursor: default;", "");
                f.WriteLine("            }}", "");
                f.WriteLine("    </style>", "");
                f.WriteLine("", "");
            }
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<nav aria-label=\"breadcrumb\">", "");
            f.WriteLine("    <ol class=\"breadcrumb shadow p-3 mb-3 bg-white rounded\">", "");
            f.WriteLine("        <li class=\"breadcrumb-item\"><a href=\"@Url.Action(\"Index\",\"{0}\")\">{1} List</a></li>", entity.Name, entity.DisplayName);
            f.WriteLine("        <li class=\"breadcrumb-item active\" aria-current=\"page\">Edit</li>", entity.DisplayName);
            f.WriteLine("    </ol>", "");
            f.WriteLine("</nav>", "");
            f.WriteLine("", "");
            f.WriteLine("<div class=\"card border-0 shadow-lg my-1\">", "");
            f.WriteLine("", "");
            f.WriteLine("    <div class=\"card-body p-0\">", "");
            f.WriteLine("        <!-- Nested Row within Card Body -->", "");
            f.WriteLine("        <div class=\"row\">", "");
            f.WriteLine("", "");
            f.WriteLine("            <div class=\"col-lg-12\">", "");
            f.WriteLine("                <div class=\"p-5\">", "");
            f.WriteLine("                    <div class=\"text-center\">", "");
            f.WriteLine("                        <h1 class=\"h4 text-gray-900 mb-4\"><u>{0}</u></h1>", entity.EditScreenTitle);
            f.WriteLine("                    </div>", "");
            if (entity.Properties.Where(o => o.VariableType == eVariableType.FILE).Count() > 0)
            {
                f.WriteLine("                    @using (Html.BeginForm(\"Edit\", \"{0}\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\", enctype = \"multipart/form-data\" }}))", entity.Name);
            }
            else
            {
                f.WriteLine("                    @using (Html.BeginForm(\"Edit\", \"{0}\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\" }}))", entity.Name);
            }
            f.WriteLine("                    {{", "");
            f.WriteLine("                        @Html.AntiForgeryToken()", "");
            f.WriteLine("                        <div class=\"form-row\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType == eVariableType.BOOL)
                    continue;

                if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                            <div class=\"form-group col-md-12\">", "");
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                }

                if (property.IsRequired)
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}</label>", property.Name, property.DisplayName);

                if (property.VariableType == eVariableType.STRING)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.PHONE)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                                @Html.TextAreaFor(m => m.{0}, new {{ @class = \"form-control\", @style = \"height:150px\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATEONLY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy}}\", new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy hh:mm:ss}}\", new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.INT)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @type = \"number\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.MONEY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:0.00}}\", new {{ @class = \"form-control\", @type = \"number\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.BOOL)
                {
                    f.WriteLine("                                <div class=\"form-check\">", property.Name);
                    f.WriteLine("                                   @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"form-check-input\" }})", property.Name);
                    f.WriteLine("                                   @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                    f.WriteLine("                                </div>", property.Name);
                }
                else if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                                <label for=\"{1}Data\" class=\"font-weight-bold\">{0}</label>", property.DisplayName, property.Name);
                    f.WriteLine("                                <div class=\"input-group\">", "");
                    f.WriteLine("                                    <input type=\"text\" name=\"{0}Name\" class=\"form-control\" placeholder=\"No file selected\" readonly value=\"@Model.{0}Name\">", property.Name);
                    f.WriteLine("                                    @Html.HiddenFor(m => m.{0}Id)", property.Name);
                    f.WriteLine("                                    <input type=\"button\" class=\"btn-danger\" onclick=\"this.form.{0}Id.value = ''; this.form.{0}Name.value = '';\" value=\"x\"  style = \"border-radius: 0\"/>", property.Name);
                    f.WriteLine("                                    <span class=\"input-group-btn btn btn-primary  custom-file-uploader\" style = \"border-radius: 0\">", "");
                    f.WriteLine("                                        @Html.TextBoxFor(m => m.{0}Data, new {{ @type = \"file\", @class = \"form-control\", @onchange = \"this.form.{0}Name.value = this.files.length ? this.files[0].name : ''\" }})", property.Name);
                    f.WriteLine("                                        Select a file", "");
                    f.WriteLine("                                    </span>", "");
                    f.WriteLine("                                </div>", "");
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}Data\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                else
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\" }})", property.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                }
                f.WriteLine("                            </div>", "");
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsHidden)
                {
                    f.WriteLine("                            @Html.HiddenFor(m => m.{0}Id)", relatedEntity.Name);
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                    if (relatedEntity.IsRequired)
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", relatedEntity.Name, relatedEntity.DisplayName);
                    else
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}</label>", relatedEntity.Name, relatedEntity.DisplayName);

                    f.WriteLine("                                @Html.DropDownList(\"{0}Id\", (SelectList)ViewBag.{0}List, \"--Select--\", new {{ @class = \"form-control selectpicker\", @data_live_search = \"true\" }})", relatedEntity.Name);
                    f.WriteLine("                                @Html.ValidationMessage(\"{0}Id\", new {{ @class = \"text-danger\" }})", relatedEntity.Name);
                    f.WriteLine("                            </div>", "");
                }
            }
            f.WriteLine("                        </div>", "");

            if (entity.Properties.Count(o => o.VariableType == eVariableType.BOOL) > 0)
                f.WriteLine("                        <hr class=\"mb-4\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType != eVariableType.BOOL)
                    continue;
                f.WriteLine("                        <div class=\"form-row\">", "");
                f.WriteLine("                            <div class=\"custom-control custom-checkbox\">", "");
                f.WriteLine("                                @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"custom-control-input\" }})", property.Name);
                if (property.IsRequired)
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}</label>", property.Name, property.DisplayName);
                f.WriteLine("                                @Html.ValidationMessage(\"{0}\", new {{ @class = \"text-danger\" }})", property.Name);
                f.WriteLine("                            </div>", "");
                f.WriteLine("                        </div>", "");
            }
            f.WriteLine("                        <hr class=\"mb-4\">", "");
            f.WriteLine("                        <button type=\"submit\" class=\"btn btn-primary\">Save</button>", "");
            f.WriteLine("                        <a class=\"btn btn-primary\" href=\"@ViewBag.ReturnUrl\">Cancel</a>", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("</div>", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("@section scripts{{", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/js/bootstrap-select.min.js\"></script>", "");
            }
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.full.min.js\"", "");
                f.WriteLine("            integrity=\"sha256-FEqEelWI3WouFOo2VWP/uJfs1y8KJ++FLh2Lbqc8SJk=\" crossorigin=\"anonymous\"></script>", "");
                f.WriteLine("    <script type=\"text/javascript\">", "");
                f.WriteLine("", "");
                f.WriteLine("        $(function () {{", "");
                foreach (var property in entity.Properties)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y',", "");
                        f.WriteLine("                timepicker: false,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y h:m:s',", "");
                        f.WriteLine("                timepicker: true,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                }
                f.WriteLine("        }});", "");
                f.WriteLine("", "");
                f.WriteLine("        $(document).ready(function () {{", "");
                foreach (var property in entity.Properties)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y',", "");
                        f.WriteLine("                timepicker: false,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y h:m:s',", "");
                        f.WriteLine("                timepicker: true,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                }
                f.WriteLine("        }});", "");
                f.WriteLine("    </script>", "");
            }
            f.WriteLine("}}", "");
        }
        private void CreateDetailFile(StreamWriter f, Application application, Entity entity)
        {
            List<Entity> entitiesForBreadCrumb = new List<Entity>();
            entitiesForBreadCrumb.Add(entity);
            BreadCrumbObjectGenerator(entity, ref entitiesForBreadCrumb);

            f.WriteLine("@model {0}.ViewModels.{1}DetailViewModel", application.Name, entity.Name);
            f.WriteLine("@using {0}.Models", application.Name);
            f.WriteLine("@{{", "");
            f.WriteLine("    ViewBag.Title = \"{0} Detail\";", entity.DisplayName);
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("@section styles{{", "");
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.min.css\"", "");
                f.WriteLine("          integrity=\"sha256-DOS9W6NR+NFe1fUhEE0PGKY/fubbUCnOfTje2JMDw3Y=\" crossorigin=\"anonymous\" />", "");
            }
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/css/bootstrap-select.min.css\" rel=\"stylesheet\" />", "");
            }
            if (entity.HasManyEntityOfThisTypeList.Count > 0)
            {
                f.WriteLine("    <link href=\"//cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css\" rel=\"stylesheet\" />", "");
                f.WriteLine("    <link href=\"//cdn.datatables.net/select/1.3.0/css/select.dataTables.min.css\" rel=\"stylesheet\" />", "");
                f.WriteLine("    <style>", "");
                f.WriteLine("        table {{", "");
                f.WriteLine("            visibility: hidden;", "");
                f.WriteLine("        }}", "");
                f.WriteLine("    </style>", "");
            }
            f.WriteLine("", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<nav aria-label=\"breadcrumb\">", "");
            f.WriteLine("    <ol class=\"breadcrumb shadow p-3 mb-3 bg-white rounded\">", "");
            if (entity.ForeignKeyEntities.Count == 0 || entitiesForBreadCrumb.Count == 1)
            {
                f.WriteLine("        <li class=\"breadcrumb-item\"><a href=\"@Url.Action(\"Index\",\"{0}\")\">{1} List</a></li>", entity.Name, entity.DisplayName);
            }
            else
            {
                f.WriteLine("        @{{", "");
                for (int i = 0; i < entitiesForBreadCrumb.Count; i++)
                {
                    if (i == 0)
                        f.WriteLine("            {0}Model {1} = ((ApplicationDbContext)ViewBag.db).{0}s.FirstOrDefault(o => o.Id == Model.Id);", entitiesForBreadCrumb[i].Name, entitiesForBreadCrumb[i].Name.ToLower());
                    else
                    {
                        f.WriteLine("            {0}Model {1} = null;", entitiesForBreadCrumb[i].Name, entitiesForBreadCrumb[i].Name.ToLower());
                        f.WriteLine("            if ({0} != null)", entitiesForBreadCrumb[i - 1].Name.ToLower());
                        f.WriteLine("            {{", "");
                        f.WriteLine("                {0} = ((ApplicationDbContext)ViewBag.db).{1}s.FirstOrDefault(o => o.Id == {2}.{1}Id);", entitiesForBreadCrumb[i].Name.ToLower(), entitiesForBreadCrumb[i].Name, entitiesForBreadCrumb[i - 1].Name.ToLower());
                        f.WriteLine("            }}", "");

                        //f.WriteLine("            {0}Model {1} = ((ApplicationDbContext)ViewBag.db).{0}s.FirstOrDefault(o => o.Id == {2}.{0}Id);", entitiesForBreadCrumb[i].Name, entitiesForBreadCrumb[i].Name.ToLower(), entitiesForBreadCrumb[i - 1].Name.ToLower());
                    }
                }
                f.WriteLine("        }}", "");
                for (int i = entitiesForBreadCrumb.Count - 1; i >= 1; i--)
                {
                    f.WriteLine("        @if (@{0} != null)", entitiesForBreadCrumb[i].Name.ToLower());
                    f.WriteLine("        {{", "");
                    f.WriteLine("           <li class=\"breadcrumb-item\"><a href=\"@Url.Action(\"Detail\", \"{0}\", new {{ @Id = @{1}.Id }})#nav-{3}\"><span style=\"color:black;font-size:0.7em\">({2})</span>@{1}.Name</a></li>", entitiesForBreadCrumb[i].Name, entitiesForBreadCrumb[i].Name.ToLower(), entitiesForBreadCrumb[i].DisplayName, entitiesForBreadCrumb[i - 1].Name.ToLower());
                    f.WriteLine("        }}", "");

                    
                }
            }
            f.WriteLine("        <li class=\"breadcrumb-item active\" aria-current=\"page\"><span style=\"color:black;font-size:0.7em\">({1})</span>@Model.Name</li>", entity.Name, entity.DisplayName);
            f.WriteLine("    </ol>", "");
            f.WriteLine("</nav>", "");
            f.WriteLine("", "");
            f.WriteLine("<div class=\"card border-0 shadow-lg my-1\">", "");
            f.WriteLine("", "");
            f.WriteLine("    <div class=\"card-body p-0\">", "");
            f.WriteLine("        <!-- Nested Row within Card Body -->", "");
            f.WriteLine("        <div class=\"row\">", "");
            f.WriteLine("", "");
            f.WriteLine("            <div class=\"col-lg-12\">", "");
            f.WriteLine("                <div class=\"p-5\">", "");
            f.WriteLine("                    <div class=\"text-center\">", "");
            f.WriteLine("                        <h1 class=\"h4 text-gray-900 mb-4\"><u>{0}</u></h1>", entity.DetailScreenTitle);
            f.WriteLine("                    </div>", "");
            f.WriteLine("					<nav>", "");
            f.WriteLine("                        <div class=\"nav nav-tabs\" id=\"nav-tab\" role=\"tablist\">", "");
            f.WriteLine("                            <a class=\"nav-item nav-link active\" id=\"nav-detail-tab\" data-toggle=\"tab\" href=\"#nav-detail\" role=\"tab\" aria-controls=\"nav-home\" aria-selected=\"true\">Detail</a>", "");
            foreach (var relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                if (!relatedEntity.ShowInTab)
                    continue;
                f.WriteLine("                            <a class=\"nav-item nav-link\" id=\"nav-{1}-tab\" data-toggle=\"tab\" href=\"#nav-{1}\" role=\"tab\" aria-controls=\"nav-{1}\" aria-selected=\"false\">{2} <span class=\"badge badge-primary\" id=\"{1}Count\">@ViewBag.{0}Count</span></a>", relatedEntity.Name, relatedEntity.Name.ToLower(), relatedEntity.DisplayName);
            }
            f.WriteLine("                        </div>", "");
            f.WriteLine("                    </nav>", "");
            f.WriteLine("					<div class=\"tab-content\" id=\"nav-tabContent\">", "");
            f.WriteLine("                        <div class=\"tab-pane fade show active\" id=\"nav-detail\" role=\"tabpanel\" aria-labelledby=\"nav-detail-tab\">", "");
            f.WriteLine("                        <div class=\"form-row mt-md-5\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType == eVariableType.BOOL)
                    continue;

                if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                            <div class=\"form-group col-md-12\">", "");
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                }

                if (property.IsRequired)
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label for=\"{0}\" class=\"font-weight-bold\">{1}</label>", property.Name, property.DisplayName);

                if (property.VariableType == eVariableType.STRING)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.PHONE)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.STRING_TEXTAREA)
                {
                    f.WriteLine("                                @Html.TextAreaFor(m => m.{0}, new {{ @class = \"form-control\", @disabled = \"disabled\", @style = \"height:150px\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATEONLY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy}}\", new {{ @class = \"form-control\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.DATETIME)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:MM/dd/yyyy hh:mm:ss}}\", new {{ @class = \"form-control\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.INT)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @type = \"number\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.MONEY)
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, \"{{0:0.00}}\", new {{ @class = \"form-control\", @type = \"number\", @disabled = \"disabled\" }})", property.Name);
                }
                else if (property.VariableType == eVariableType.BOOL)
                {
                    f.WriteLine("                                <div class=\"form-check\">", property.Name);
                    f.WriteLine("                                   @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"form-check-input\", @disabled = \"disabled\" }})", property.Name);
                    f.WriteLine("                                </div>", property.Name);
                }
                else if (property.VariableType == eVariableType.FILE)
                {
                    f.WriteLine("                                @if (Model.{0}Id == null)", property.Name);
                    f.WriteLine("                                {{", "");
                    f.WriteLine("                                   <input type=\"text\" disabled value=\"No file Chosen\" class=\"form-control\"/>", "");
                    f.WriteLine("                                }}", "");
                    f.WriteLine("                                else", "");
                    f.WriteLine("                                {{", "");
                    f.WriteLine("                                   <a href=\"@Url.Action(\"DownloadFile\",\"File\", new {{ @Id = Model.{0}Id }})\" class=\"form-control\">@Model.{0}Name</a>", property.Name);
                    f.WriteLine("                                }}", "");
                }
                else
                {
                    f.WriteLine("                                @Html.TextBoxFor(m => m.{0}, new {{ @class = \"form-control\", @disabled = \"disabled\" }})", property.Name);
                }
                f.WriteLine("                            </div>", "");
            }
            foreach (var relatedEntity in entity.ForeignKeyEntities)
            {
                if (relatedEntity.IsHidden)
                {
                    f.WriteLine("                            @Html.HiddenFor(m => m.{0}Id)", relatedEntity.Name);
                }
                else
                {
                    f.WriteLine("                            <div class=\"form-group col-md-4\">", "");
                    if (relatedEntity.IsRequired)
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}<span class = \"text-danger\">*</span></label>", relatedEntity.Name, relatedEntity.DisplayName);
                    else
                        f.WriteLine("                                <label for=\"{0}Id\" class=\"font-weight-bold\">{1}</label>", relatedEntity.Name, relatedEntity.DisplayName);
                    f.WriteLine("                                @Html.DropDownList(\"{0}Id\", (SelectList)ViewBag.{0}List, \"--Select--\", new {{ @class = \"form-control selectpicker\", @data_live_search = \"true\", @disabled = \"disabled\" }})", relatedEntity.Name);
                    f.WriteLine("                            </div>", "");
                }
            }
            f.WriteLine("                        </div>", "");

            if (entity.Properties.Count(o => o.VariableType == eVariableType.BOOL) > 0)
                f.WriteLine("                        <hr class=\"mb-4\">", "");
            foreach (var property in entity.Properties)
            {
                if (property.VariableType != eVariableType.BOOL)
                    continue;
                f.WriteLine("                        <div class=\"form-row\">", "");
                f.WriteLine("                            <div class=\"custom-control custom-checkbox\">", "");
                f.WriteLine("                                   @Html.CheckBoxFor(m => m.{0}, new {{ @class = \"custom-control-input\", @disabled = \"disabled\" }})", property.Name);
                if (property.IsRequired)
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}<span class = \"text-danger\">*</span></label>", property.Name, property.DisplayName);
                else
                    f.WriteLine("                                <label class=\"custom-control-label\" for=\"{0}\">{1}</label>", property.Name, property.DisplayName);
                f.WriteLine("                            </div>", "");
                f.WriteLine("                        </div>", "");
            }
            f.WriteLine("                        <hr class=\"mb-4\">", "");
            f.WriteLine("                        <button class=\"btn btn-primary\" onclick=\"location.href = '@Url.Action(\"Edit\", new{{ @id = Model.Id, @returnUrl = Request.Url.AbsoluteUri}})'\">Edit</button>", "");
            f.WriteLine("                        @if(ViewBag.ReturnUrl == null)", "");
            f.WriteLine("                        {{", "");
            f.WriteLine("                           <button class=\"btn btn-primary\" onclick=\"location.href = '@Url.Action(\"Index\", \"{0}\")'\">Back</button>", entity.Name);
            f.WriteLine("                        }}", "");
            f.WriteLine("                        else", "");
            f.WriteLine("                        {{", "");
            f.WriteLine("                           <button class=\"btn btn-primary\" onclick=\"location.href = '@ViewBag.ReturnUrl'\">Back</button>", "");
            f.WriteLine("                        }}", "");
            //if (entity.ForeignKeyEntities.Count == 0 || entitiesForBreadCrumb.Count == 1)
            //{
            //    f.WriteLine("                        @if(ViewBag.ReturnUrl == null)", "");
            //    f.WriteLine("                        {{", "");
            //    f.WriteLine("                           <button class=\"btn btn-primary\" onclick=\"location.href = '@Url.Action(\"Index\", \"{0}\")'\">Back</button>", entity.Name);
            //    f.WriteLine("                        }}", "");
            //    f.WriteLine("                        else", "");
            //    f.WriteLine("                        {{", "");
            //    f.WriteLine("                           <button class=\"btn btn-primary\" onclick=\"location.href = '@ViewBag.ReturnUrl'\">Back</button>", "");
            //    f.WriteLine("                        }}", "");
            //}
            //else
            //{
            //    f.WriteLine("                        <button class=\"btn btn-primary\" onclick=\"location.href = '@Url.Action(\"Detail\", \"{0}\", new{{ @id = @{1}.Id}})#nav-{2}'\">Back</button>", entitiesForBreadCrumb[1].Name, entitiesForBreadCrumb[1].Name.ToLower(), entity.Name.ToLower());
            //}
            f.WriteLine("                        </div>", "");
            foreach (var relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                if (!relatedEntity.ShowInTab)
                    continue;
                f.WriteLine("                        <div class=\"tab-pane fade\" id=\"nav-{0}\" role=\"tabpanel\" aria-labelledby=\"nav-{0}-tab\">", relatedEntity.Name.ToLower());
                f.WriteLine("                            <div class=\"d-flex mt-md-2\">", "");
                f.WriteLine("                                <div class=\"ml-auto\">", "");
                if (relatedEntity.Name == relatedEntity.Entity.Name)
                {
                    f.WriteLine("                                    <a class=\"btn btn-success btn-icon-split btn-sm\" href=\"@Url.Action(\"Add\", \"{0}\", new {{ @{1}Id = Model.Id, @ReturnUrl = Request.RawUrl.ToString() + \"#nav-{3}\" }})\">", relatedEntity.Entity.Name, entity.Name, relatedEntity.Entity.Name.ToLower(), relatedEntity.Name.ToLower(), relatedEntity.Name);
                }
                else
                {
                    f.WriteLine("                                    <a class=\"btn btn-success btn-icon-split btn-sm\" href=\"@Url.Action(\"Add\", \"{0}\", new {{ @{4}Id = Model.Id, @ReturnUrl = Request.RawUrl.ToString() + \"#nav-{3}\" }})\">", relatedEntity.Entity.Name, entity.Name, relatedEntity.Entity.Name.ToLower(), relatedEntity.Name.ToLower(), relatedEntity.Name);
                }
                f.WriteLine("                                        <span class=\"icon text-white-50\">", "");
                f.WriteLine("                                            <i class=\"fas fa-plus\"></i>", "");
                f.WriteLine("                                        </span>", "");
                f.WriteLine("                                        <span class=\"text\">Add New</span>", "");
                f.WriteLine("                                    </a>", "");
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0 && relatedEntity.Entity != entity)
                {
                    f.WriteLine("                                    <button id=\"btnAdd{0}\" class=\"btn btn-primary btn-icon-split btn-sm\">", relatedEntity.Name);
                    f.WriteLine("                                        <span class=\"icon text-white-50\">", "");
                    f.WriteLine("                                            <i class=\"fas fa-plus\"></i>", "");
                    f.WriteLine("                                        </span>", "");
                    f.WriteLine("                                        <span class=\"text\">Add</span>", "");
                    f.WriteLine("                                    </button>", "");
                }
                f.WriteLine("                                </div>", "");
                f.WriteLine("                            </div>", "");
                f.WriteLine("                            <div class=\"table-responsive mt-md-2\">", "");
                f.WriteLine("                                <table class=\"table table-bordered\" id=\"tbl{0}\" width=\"100%\" cellspacing=\"0\">", relatedEntity.Name);
                f.WriteLine("                                    <thead>", "");
                f.WriteLine("                                        <tr>", "");
                f.WriteLine("                                            <th>Id</th>", "");
                foreach (var prop in relatedEntity.Entity.Properties)
                {
                    if (prop.IsVisibleInListingScreen)
                    {
                        f.WriteLine("                                            <th>{0}</th>", prop.DisplayName);
                    }
                }
                foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                {
                    if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                    {
                        f.WriteLine("                                            <th>{0}</th>", fke.DisplayName);
                    }
                }
                f.WriteLine("                                            <th>Command</th>", "");
                f.WriteLine("                                        </tr>", "");
                f.WriteLine("                                    </thead>", "");
                f.WriteLine("                                </table>", "");
                f.WriteLine("                            </div>", "");
                f.WriteLine("                        </div>", "");
            }
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("</div>", "");
            f.WriteLine("", "");
            foreach (var relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                if (!relatedEntity.ShowInTab)
                    continue;
                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("<div id=\"Add{0}Model\" class=\"modal fade\">", relatedEntity.Entity.Name);
                    f.WriteLine("    <div class=\"modal-dialog modal-lg\">", "");
                    f.WriteLine("        <div class=\"modal-content\">", "");
                    f.WriteLine("            <div class=\"modal-header bg-primary\">", "");
                    f.WriteLine("                <h4 class=\"modal-title text-white\">Add {0}</h4>", relatedEntity.Entity.Name);
                    f.WriteLine("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>", "");
                    f.WriteLine("            </div>", "");
                    f.WriteLine("            <div class=\"modal-body\">", "");
                    f.WriteLine("                <div class=\"table-responsive mt-md-2\">", "");
                    f.WriteLine("                    <table class=\"table table-bordered\" id=\"tblAdd{0}\" width=\"100%\" cellspacing=\"0\">", relatedEntity.Entity.Name);
                    f.WriteLine("                        <thead>", "");
                    f.WriteLine("                            <tr>", "");
                    f.WriteLine("                                <th></th>", "");
                    f.WriteLine("                                <th>Id</th>", "");
                    foreach (var prop in relatedEntity.Entity.Properties)
                    {
                        if (prop.IsVisibleInListingScreen)
                        {
                            f.WriteLine("                                            <th>{0}</th>", prop.DisplayName);
                        }
                    }
                    foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                    {
                        if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                        {
                            f.WriteLine("                                            <th>{0}</th>", fke.DisplayName);
                        }
                    }
                    f.WriteLine("                            </tr>", "");
                    f.WriteLine("                        </thead>", "");
                    f.WriteLine("                    </table>", "");
                    f.WriteLine("                </div>", "");
                    f.WriteLine("            </div>", "");
                    f.WriteLine("            <div class=\"modal-footer\">", "");
                    f.WriteLine("                <button type=\"button\" id=\"btnAdd{0}Model\" class=\"btn btn-primary\">Add</button>", relatedEntity.Entity.Name);
                    f.WriteLine("                <button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\">Close</button>", "");
                    f.WriteLine("            </div>", "");
                    f.WriteLine("        </div>", "");
                    f.WriteLine("", "");
                    f.WriteLine("    </div>", "");
                    f.WriteLine("</div>", "");
                    f.WriteLine("", "");
                }
            }
            f.WriteLine("@section scripts{{", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            if (entity.ForeignKeyEntities.Count() > 0)
            {
                f.WriteLine("    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap-select@1.13.9/dist/js/bootstrap-select.min.js\"></script>", "");
            }
            if (entity.HasManyEntityOfThisTypeList.Count > 0)
            {
                f.WriteLine("    <script src=\"//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js\"></script>", "");
                f.WriteLine("    <script src=\"//cdn.datatables.net/select/1.3.0/js/dataTables.select.min.js\"></script>", "");
            }
            if (entity.Properties.Count(o => o.VariableType == eVariableType.DATETIME || o.VariableType == eVariableType.DATEONLY) > 0)
            {
                f.WriteLine("    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/jquery-datetimepicker/2.5.20/jquery.datetimepicker.full.min.js\"", "");
                f.WriteLine("            integrity=\"sha256-FEqEelWI3WouFOo2VWP/uJfs1y8KJ++FLh2Lbqc8SJk=\" crossorigin=\"anonymous\"></script>", "");
                f.WriteLine("    <script type=\"text/javascript\">", "");
                f.WriteLine("", "");
                f.WriteLine("        $(function () {{", "");
                foreach (var property in entity.Properties)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y',", "");
                        f.WriteLine("                timepicker: false,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y h:m:s',", "");
                        f.WriteLine("                timepicker: true,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                }
                f.WriteLine("        }});", "");
                f.WriteLine("", "");
                f.WriteLine("        $(document).ready(function () {{", "");
                foreach (var property in entity.Properties)
                {
                    if (property.VariableType == eVariableType.DATEONLY)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y',", "");
                        f.WriteLine("                timepicker: false,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                    if (property.VariableType == eVariableType.DATETIME)
                    {
                        f.WriteLine("            $('#{0}').datetimepicker({{", property.Name);
                        f.WriteLine("                format: 'm/d/Y h:m:s',", "");
                        f.WriteLine("                timepicker: true,", "");
                        f.WriteLine("                onChangeDateTime: function (dp, $input) {{", "");
                        f.WriteLine("", "");
                        f.WriteLine("                }}", "");
                        f.WriteLine("            }});", "");
                    }
                }
                f.WriteLine("        }});", "");
                f.WriteLine("    </script>", "");
            }

            foreach (var relatedEntity in entity.HasManyEntityOfThisTypeList)
            {
                if (!relatedEntity.ShowInTab)
                    continue;
                f.WriteLine("    <script>", "");
                f.WriteLine("    var {0}Table = null;", relatedEntity.Name.ToLower());
                f.WriteLine("    $(\"#nav-{0}-tab\").click(function () {{", relatedEntity.Name.ToLower());
                f.WriteLine("        if ({0}Table == null) {{", relatedEntity.Name.ToLower());
                f.WriteLine("            {0}Table = $('#tbl{1}').DataTable({{", relatedEntity.Name.ToLower(), relatedEntity.Name);
                f.WriteLine("                \"columnDefs\": [", "");
                f.WriteLine("                    {{", "");
                f.WriteLine("                        \"targets\": 0,", "");
                f.WriteLine("                        \"visible\": false,", "");
                f.WriteLine("                        \"searchable\": false", "");
                f.WriteLine("                    }},", "");
                f.WriteLine("                    {{", "");
                f.WriteLine("                        \"targets\": -1,", "");
                f.WriteLine("                        \"width\": \"100px\",", "");
                f.WriteLine("                        \"className\": \"text-center\",", "");
                f.WriteLine("                        \"data\": null,", "");
                f.WriteLine("                        \"orderable\": false,", "");
                f.WriteLine("                        \"defaultContent\": `<button data-value=\"detail\" class=\"btn btn-primary btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Detail!\">", "");
                f.WriteLine("                                                                <i class=\"fa fa-info\" aria-hidden=\"true\"></i>", "");
                f.WriteLine("                                                           </button>&nbsp;", "");
                f.WriteLine("                                                           <button data-value=\"edit\" class=\"btn btn-warning btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Edit!\">", "");
                f.WriteLine("                                                                <i class=\"fa fa-edit\" aria-hidden=\"true\"></i>", "");
                f.WriteLine("                                                            </button>&nbsp;", "");
                f.WriteLine("                                                           <button data-value=\"remove\" class=\"btn btn-danger btn-circle btn-sm\" data-tooltip=\"tooltip\" title=\"Remove!\">", "");
                if (entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == relatedEntity.Entity).Count() > 0 && relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("                                                                <i class=\"fa fa-minus\" aria-hidden=\"true\"></i>", "");
                }
                else
                {
                    f.WriteLine("                                                                <i class=\"fa fa-trash\" aria-hidden=\"true\"></i>", "");
                }
                f.WriteLine("                                                            </button>`", "");
                f.WriteLine("                    }}", "");
                f.WriteLine("                ]", "");
                f.WriteLine("            }});", "");
                f.WriteLine("", "");
                f.WriteLine("            $.post(\"/{0}/Get{1}List\", {{ id: '@Model.Id' }}, function (data, status) {{", entity.Name, relatedEntity.Name);
                f.WriteLine("                for (var i = 0; i < data.length; i++) {{", "");
                f.WriteLine("                    var obj = data[i];", "");
                f.WriteLine("                    {0}Table.row.add([", relatedEntity.Name.ToLower());
                f.WriteLine("                        obj.Id,", "");
                foreach (var prop in relatedEntity.Entity.Properties)
                {
                    if (prop.IsVisibleInListingScreen)
                    {
                        f.WriteLine("                        obj.{0},", prop.Name);
                    }
                }
                foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                {
                    if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                    {
                        f.WriteLine("                        obj.{0}String,", fke.Name);
                    }
                }
                f.WriteLine("                        \"\"", "");
                f.WriteLine("                    ]).draw(false);", "");
                f.WriteLine("                }}", "");
                f.WriteLine("            }});", "");
                f.WriteLine("", "");
                f.WriteLine("            $(\"#tbl{0}\").attr(\"style\", \"visibility: visible\");", relatedEntity.Name);
                f.WriteLine("", "");
                f.WriteLine("            $('#tbl{0} tbody').on('dblclick', 'tr', function () {{", relatedEntity.Name);
                f.WriteLine("                var data = {0}Table.row($(this)).data();", relatedEntity.Name.ToLower());
                f.WriteLine("                window.location.href = \"@Url.Action(\"Detail\",\"{0}\", new {{ id = \"\" }})/\" + data[0] + \"?returnUrl=\" + \"@Request.Url.AbsoluteUri\";", relatedEntity.Entity.Name);
                f.WriteLine("            }});", "");
                f.WriteLine("            $('#tbl{0} tbody').on('click', 'button', function () {{", relatedEntity.Name);
                f.WriteLine("                var action = this.getAttribute(\"data-value\");", "");
                f.WriteLine("                var data = {0}Table.row($(this).parents('tr')).data();", relatedEntity.Name.ToLower());
                f.WriteLine("", "");
                f.WriteLine("                if (action == 'detail') {{", "");
                f.WriteLine("                    window.location.href = \"@Url.Action(\"Detail\",\"{0}\")?id=\" + data[0] + \"&returnUrl=\" + \"@Request.Url.AbsoluteUri\";", relatedEntity.Entity.Name);
                f.WriteLine("                }}", "");
                f.WriteLine("", "");
                f.WriteLine("                if (action == 'edit') {{", "");
                f.WriteLine("                    window.location.href = \"@Url.Action(\"Edit\",\"{0}\")/\" + data[0] + \"?returnUrl=\" + \"@Request.Url.AbsoluteUri\" + \"%23nav-{2}\";", relatedEntity.Entity.Name, relatedEntity.Entity.Name.ToLower(), relatedEntity.Name.ToLower());
                f.WriteLine("                }}", "");
                f.WriteLine("", "");
                f.WriteLine("                if (action == 'remove') {{", "");
                f.WriteLine("                    if (!confirm(\"Are you sure you wish to remove?\")) {{", "");
                f.WriteLine("                        return;", "");
                f.WriteLine("                    }}", "");
                f.WriteLine("                    var row = {0}Table.row($(this).parents('tr'));", relatedEntity.Name.ToLower());
                f.WriteLine("                    var request = window.location.href;", "");
                f.WriteLine("                    var jqxhr = $.post(\"/{0}/Remove{4}\", {{ {3}Id: data[0], {1}Id: '@Model.Id' }}, function (result) {{", entity.Name, entity.Name.ToLower(), relatedEntity.Entity.Name, relatedEntity.Name.ToLower(), relatedEntity.Name);
                f.WriteLine("                        row.remove().draw();", "");
                f.WriteLine("                        $(\"#{0}Count\").html({0}Table.rows().count());", relatedEntity.Name.ToLower());
                f.WriteLine("                    }})", "");
                f.WriteLine("                        .fail(function () {{", "");
                f.WriteLine("                            alert(\"Error updating data, please try again after sometimes\");", "");
                f.WriteLine("                        }})", "");
                f.WriteLine("                }}", "");
                f.WriteLine("            }});", "");
                f.WriteLine("        }}", "");
                f.WriteLine("        else {{", "");
                f.WriteLine("            $(\"#{0}Count\").html({0}Table.rows().count());", relatedEntity.Name.ToLower());
                f.WriteLine("        }}", "");
                f.WriteLine("    }});", "");
                f.WriteLine("", "");

                if (relatedEntity.Entity.HasManyEntityOfThisTypeList.Where(o => o.Entity == entity).Count() > 0)
                {
                    f.WriteLine("    var add{0}Table = null;", relatedEntity.Name);
                    f.WriteLine("    $('#btnAdd{0}').on('click', function () {{", relatedEntity.Name);
                    f.WriteLine("        if (add{0}Table == null) {{", relatedEntity.Name);
                    f.WriteLine("            add{0}Table = $('#tblAdd{0}').DataTable({{", relatedEntity.Name);
                    f.WriteLine("                \"columnDefs\": [", "");
                    f.WriteLine("                    {{", "");
                    f.WriteLine("                        \"targets\": [1],", "");
                    f.WriteLine("                        \"visible\": false,", "");
                    f.WriteLine("                        \"searchable\": false", "");
                    f.WriteLine("                    }},", "");
                    f.WriteLine("                    {{", "");
                    f.WriteLine("                        orderable: false,", "");
                    f.WriteLine("                        className: 'select-checkbox',", "");
                    f.WriteLine("                        targets: 0", "");
                    f.WriteLine("                    }}", "");
                    f.WriteLine("                ],", "");
                    f.WriteLine("                select: {{", "");
                    f.WriteLine("                    style: 'multi',", "");
                    f.WriteLine("                    selector: 'td:first-child'", "");
                    f.WriteLine("                }},", "");
                    f.WriteLine("                order: [[2, 'asc']]", "");
                    f.WriteLine("            }});", "");
                    f.WriteLine("", "");
                    f.WriteLine("            $.post(\"/{0}/GetNotAdded{1}List\", {{ id: '@Model.Id' }}, function (data, status) {{", entity.Name, relatedEntity.Name);
                    f.WriteLine("                for (var i = 0; i < data.length; i++) {{", "");
                    f.WriteLine("                    var obj = data[i];", "");
                    f.WriteLine("                    add{0}Table.row.add([", relatedEntity.Name);
                    f.WriteLine("                        \"\",", "");
                    f.WriteLine("                        obj.Id,", "");
                    foreach (var prop in relatedEntity.Entity.Properties)
                    {
                        if (prop.IsVisibleInListingScreen)
                        {
                            f.WriteLine("                        obj.{0},", prop.Name);
                        }
                    }
                    foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                    {
                        if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                        {
                            f.WriteLine("                        obj.{0}String,", fke.Name);
                        }
                    }
                    f.WriteLine("                    ]).draw(false);", "");
                    f.WriteLine("                }}", "");
                    f.WriteLine("            }});", "");
                    f.WriteLine("", "");
                    f.WriteLine("            $(\"#tblAdd{0}\").attr(\"style\", \"visibility: visible\");", relatedEntity.Entity.Name);
                    f.WriteLine("", "");
                    f.WriteLine("            $(\"#btnAdd{0}Model\").on('click', function () {{", relatedEntity.Name);
                    f.WriteLine("                var selectedRows = add{0}Table.rows({{ selected: true }}).data();", relatedEntity.Name);
                    f.WriteLine("                var added{0}s = [];", relatedEntity.Name);
                    f.WriteLine("                for (var i = 0; i < selectedRows.length; i++) {{", "");
                    f.WriteLine("                    added{0}s.push(selectedRows[i][1]);", relatedEntity.Name);
                    f.WriteLine("                }}", "");
                    f.WriteLine("                $.post(\"/{0}/Add{2}s\", {{ {3}IdList: added{2}s, {1}Id: '@Model.Id' }}, function (data, status) {{", entity.Name, entity.Name.ToLower(), relatedEntity.Name, relatedEntity.Name.ToLower());
                    f.WriteLine("                    for (var i = 0; i < selectedRows.length; i++) {{", "");
                    f.WriteLine("                        {0}Table.row.add([", relatedEntity.Entity.Name.ToLower());
                    f.WriteLine("                            selectedRows[i][1],", "");
                    for (int i = 0; i < relatedEntity.Entity.Properties.Count + relatedEntity.Entity.ForeignKeyEntities.Count - relatedEntity.Entity.ForeignKeyEntities.Where(o => o.Name == entity.Name).Count(); i++)
                    {
                        f.WriteLine("                            selectedRows[i][{0}],", i + 2);
                    }
                    f.WriteLine("                            \"\"", "");
                    f.WriteLine("                        ]).draw(false);", "");
                    f.WriteLine("                        $('#Add{0}Model').modal('hide');", relatedEntity.Name);
                    f.WriteLine("                        $(\"#{0}Count\").html({0}Table.rows().count());", relatedEntity.Name.ToLower());
                    f.WriteLine("                    }}", "");
                    f.WriteLine("                }});", "");
                    f.WriteLine("            }});", "");
                    f.WriteLine("        }}", "");
                    f.WriteLine("        else {{", "");
                    f.WriteLine("            add{0}Table.clear().draw();", relatedEntity.Name);
                    f.WriteLine("            $.post(\"/{0}/GetNotAdded{1}List\", {{ id: '@Model.Id' }}, function (data, status) {{", entity.Name, relatedEntity.Name);
                    f.WriteLine("                for (var i = 0; i < data.length; i++) {{", "");
                    f.WriteLine("                    var obj = data[i];", "");
                    f.WriteLine("                    add{0}Table.row.add([", relatedEntity.Name);
                    f.WriteLine("                        \"\",", "");
                    f.WriteLine("                        obj.Id,", "");
                    foreach (var prop in relatedEntity.Entity.Properties)
                    {
                        if (prop.IsVisibleInListingScreen)
                        {
                            f.WriteLine("                        obj.{0},", prop.Name);
                        }
                    }
                    foreach (var fke in relatedEntity.Entity.ForeignKeyEntities)
                    {
                        if (fke.IsVisibleInListingScreen && fke.Name != entity.Name)
                        {
                            f.WriteLine("                        obj.{0},", fke.Name);
                        }
                    }
                    f.WriteLine("                    ]).draw(false);", "");
                    f.WriteLine("                }}", "");
                    f.WriteLine("            }});", "");
                    f.WriteLine("        }}", "");
                    f.WriteLine("", "");
                    f.WriteLine("        $('#Add{0}Model').modal({{show:true}});", relatedEntity.Name);
                    f.WriteLine("    }});", "");
                }
                f.WriteLine("    </script>", "");
            }

            if (entity.HasManyEntityOfThisTypeList.Count > 0)
            {
                f.WriteLine("    <script>", "");
                f.WriteLine("        $(document).ready(function () {{", "");
                f.WriteLine("            $('#nav-tab a').click(function (e) {{", "");
                f.WriteLine("                e.preventDefault();", "");
                f.WriteLine("                $(this).tab('show');", "");
                f.WriteLine("            }});", "");
                f.WriteLine("", "");
                f.WriteLine("            // store the currently selected tab in the hash value", "");
                f.WriteLine("            $(\"#nav-tab a\").on(\"shown.bs.tab\", function (e) {{", "");
                f.WriteLine("                var id = $(e.target).attr(\"href\").substr(1);", "");
                f.WriteLine("                window.location.hash = id;", "");
                f.WriteLine("            }});", "");
                f.WriteLine("", "");
                f.WriteLine("            var hash = window.location.hash;", "");
                f.WriteLine("            $('#nav-tab a[href=\"' + hash + '\"]').click();", "");
                f.WriteLine("        }});", "");
                f.WriteLine("    </script>", "");
            }
            f.WriteLine("}}", "");
        }
    }
}
