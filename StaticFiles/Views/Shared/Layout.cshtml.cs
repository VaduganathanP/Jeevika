using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Views.Shared
{
    public class Layout_cshtml
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta charset=\"utf-8\" />", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">", "");
            f.WriteLine("    <title>@ViewBag.Title - {0}</title>", application.DisplayName);
            f.WriteLine("    @Styles.Render(\"~/Content/css\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/modernizr\")", "");
            f.WriteLine("</head>", "");
            f.WriteLine("<body>", "");
            f.WriteLine("    <div class=\"navbar navbar-inverse navbar-fixed-top\">", "");
            f.WriteLine("        <div class=\"container\">", "");
            f.WriteLine("            <div class=\"navbar-header\">", "");
            f.WriteLine("                <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">", "");
            f.WriteLine("                    <span class=\"icon-bar\"></span>", "");
            f.WriteLine("                    <span class=\"icon-bar\"></span>", "");
            f.WriteLine("                    <span class=\"icon-bar\"></span>", "");
            f.WriteLine("                </button>", "");
            f.WriteLine("                @Html.ActionLink(\"Application name\", \"Index\", \"Home\", new {{ area = \"\" }}, new {{ @class = \"navbar-brand\" }})", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("            <div class=\"navbar-collapse collapse\">", "");
            f.WriteLine("                <ul class=\"nav navbar-nav\">", "");
            f.WriteLine("                    <li>@Html.ActionLink(\"Home\", \"Index\", \"Home\")</li>", "");
            f.WriteLine("                    <li>@Html.ActionLink(\"About\", \"About\", \"Home\")</li>", "");
            f.WriteLine("                    <li>@Html.ActionLink(\"Contact\", \"Contact\", \"Home\")</li>", "");
            f.WriteLine("                </ul>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("    <div class=\"container body-content\">", "");
            f.WriteLine("        @RenderBody()", "");
            f.WriteLine("        <hr />", "");
            f.WriteLine("        <footer>", "");
            f.WriteLine("            <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>", "");
            f.WriteLine("        </footer>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jquery\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/bootstrap\")", "");
            f.WriteLine("    @RenderSection(\"scripts\", required: false)", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");

        }
    }
}
