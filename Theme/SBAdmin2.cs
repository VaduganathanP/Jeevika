using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.Theme
{
    public class SBAdmin2
    {
        public static void Implement(string applicationPath, Application application)
        {
            PackageManager.Theme_SBAdmin2_Mvc.Install(applicationPath, application);
            PackageManager.Jquery_Easing_Mvc.Install(applicationPath, application);
            PackageManager.Font_Awsome.Install(applicationPath, application);

            var viewSharedLayoutCshtmlFilePath = Path.Combine(applicationPath, "Views", "Shared", "_Layout.cshtml");
            using (var sw = CreateFile(viewSharedLayoutCshtmlFilePath))
            {
                Write_View_Shared_Layout_Content(sw, application);
            }
        }

        private static void Write_View_Shared_Layout_Content(StreamWriter f, Application application)
        {
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta charset=\"utf-8\" />", "");
            f.WriteLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">", "");
            f.WriteLine("    <meta name=\"description\" content=\"\">", "");
            f.WriteLine("    <meta name=\"author\" content=\"\">", "");
            f.WriteLine("    <title>@ViewBag.Title - {0}</title>", application.DisplayName);
            f.WriteLine("    <link href=\"https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i\" rel=\"stylesheet\">", "");
            f.WriteLine("    <link href=\"~/Content/fontawesome-all.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    @Styles.Render(\"~/Content/css\")", "");
            f.WriteLine("    @RenderSection(\"styles\", required: false)", "");
            f.WriteLine("    <link href=\"~/Content/fontawesome-all.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    <link href=\"~/Content/sb-admin-2.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/modernizr\")", "");
            f.WriteLine("</head>", "");
            f.WriteLine("<body id=\"page-top\">", "");
            f.WriteLine("", "");
            f.WriteLine("    <!-- Page Wrapper -->", "");
            f.WriteLine("    <div id=\"wrapper\">", "");
            f.WriteLine("        <!-- Sidebar -->", "");
            f.WriteLine("        <ul class=\"navbar-nav bg-gradient-primary sidebar sidebar-dark accordion\" id=\"accordionSidebar\">", "");
            f.WriteLine("", "");
            f.WriteLine("            <!-- Sidebar - Brand -->", "");
            f.WriteLine("            <a class=\"sidebar-brand d-flex align-items-center justify-content-center\" href=\"index.html\">", "");
            f.WriteLine("                <div class=\"sidebar-brand-icon rotate-n-15\">", "");
            f.WriteLine("                    @*<i class=\"fas fa-laugh-wink\"></i>*@", "");
            f.WriteLine("                    <img src=\"~/Images/favicon-32x32.png\" />", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("                <div class=\"sidebar-brand-text mx-1\">{0}</div>", application.DisplayName);
            f.WriteLine("            </a>", "");
            f.WriteLine("", "");
            f.WriteLine("            <!-- Divider -->", "");
            f.WriteLine("            <hr class=\"sidebar-divider my-0\">", "");
            f.WriteLine("", "");
            f.WriteLine("            <!-- Nav Item - Dashboard -->", "");
            f.WriteLine("            <li class=\"nav-item\">", "");
            f.WriteLine("                <a class=\"nav-link\" href=\"@Url.Action(\"Index\",\"Home\")\">", "");
            f.WriteLine("                    <i class=\"fas fa-fw fa-tachometer-alt\"></i>", "");
            f.WriteLine("                    <span>Dashboard</span>", "");
            f.WriteLine("                </a>", "");
            f.WriteLine("            </li>", "");
            f.WriteLine("", "");

            f.WriteLine("", "");
            f.WriteLine("            <li class=\"nav-item\">", "");
            f.WriteLine("                <a class=\"nav-link collapsed\" href=\"#\" data-toggle=\"collapse\" data-target=\"#collapseTwo\" aria-expanded=\"true\" aria-controls=\"collapseTwo\">", "");
            f.WriteLine("                    <i class=\"fas fa-fw fa-tachometer-alt\"></i>", "");
            f.WriteLine("                    <span>All Entities</span>", "");
            f.WriteLine("                </a>", "");
            f.WriteLine("                <div id=\"collapseTwo\" class=\"collapse\" aria-labelledby=\"headingTwo\" data-parent=\"#accordionSidebar\">", "");
            f.WriteLine("                    <div class=\"bg-white py-2 collapse-inner rounded\">", "");
            f.WriteLine("                        <h6 class=\"collapse-header\">Base Component:</h6>", "");
            foreach (var entity in application.Entities)
            {
                if (!entity.DisplayInDashBoardMenu && entity.DisplayInAllFiles)
                {
                    if (entity.Name == "File")
                        continue;
                    f.WriteLine("                        <a class=\"collapse-item\" href=\"@Url.Action(\"Index\",\"{0}\")\">{1}</a>", entity.Name, entity.DisplayName);
                }
            }
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </li>", "");
            f.WriteLine("", "");

            foreach (var entity in application.Entities)
            {
                if (entity.DisplayInDashBoardMenu)
                {
                    f.WriteLine("            <!-- Nav Item - {0} -->", entity.Name);
                    f.WriteLine("            <li class=\"nav-item\">", "");
                    f.WriteLine("                <a class=\"nav-link\" href=\"@Url.Action(\"Index\",\"{0}\")\">", entity.Name);
                    f.WriteLine("                    <i class=\"fas fa-fw fa-tachometer-alt\"></i>", "");
                    f.WriteLine("                    <span>{0}</span>", entity.DisplayName);
                    f.WriteLine("                </a>", "");
                    f.WriteLine("            </li>", "");
                    f.WriteLine("", "");
                }
            }
            f.WriteLine("            <!-- Divider -->", "");
            f.WriteLine("            <hr class=\"sidebar-divider d-none d-md-block\">", "");
            f.WriteLine("            <div class=\"text-center d-none d-md-inline\">", "");
            f.WriteLine("                <button class=\"rounded-circle border-0\" id=\"sidebarToggle\"></button>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("        </ul>", "");
            f.WriteLine("        <!-- End of Sidebar -->", "");
            f.WriteLine("        <!-- Content Wrapper -->", "");
            f.WriteLine("        <div id=\"content-wrapper\" class=\"d-flex flex-column\">", "");
            f.WriteLine("            <div id=\"content\">", "");
            f.WriteLine("", "");
            f.WriteLine("                <!-- Topbar -->", "");
            f.WriteLine("                <nav class=\"navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow\">", "");
            f.WriteLine("", "");
            f.WriteLine("                    <!-- Sidebar Toggle (Topbar) -->", "");
            f.WriteLine("                    <button id=\"sidebarToggleTop\" class=\"btn btn-link d-md-none rounded-circle mr-3\">", "");
            f.WriteLine("                        <i class=\"fa fa-bars\"></i>", "");
            f.WriteLine("                    </button>", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("                    <!-- Topbar Navbar -->", "");
            f.WriteLine("                    <ul class=\"navbar-nav ml-auto\">", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("                        <div class=\"topbar-divider d-none d-sm-block\"></div>", "");
            f.WriteLine("", "");
            f.WriteLine("                        <!-- Nav Item - User Information -->", "");
            f.WriteLine("                        <li class=\"nav-item dropdown no-arrow\">", "");
            f.WriteLine("                            <a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"userDropdown\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">", "");
            f.WriteLine("                                <span class=\"mr-2 d-none d-lg-inline text-gray-600 small\">@User.Identity.Name</span>", "");
            f.WriteLine("                                <i class=\"fas fa-user fa-sm fa-fw mr-2 text-gray-400\"></i>", "");
            f.WriteLine("                            </a>", "");
            f.WriteLine("                            <!-- Dropdown - User Information -->", "");
            f.WriteLine("                            <div class=\"dropdown-menu dropdown-menu-right shadow animated--grow-in\" aria-labelledby=\"userDropdown\">", "");
            f.WriteLine("                                <a class=\"dropdown-item\" href=\"#\">", "");
            f.WriteLine("                                    <i class=\"fas fa-user fa-sm fa-fw mr-2 text-gray-400\"></i>", "");
            f.WriteLine("                                    Profile", "");
            f.WriteLine("                                </a>", "");
            f.WriteLine("                                <a class=\"dropdown-item\" href=\"#\">", "");
            f.WriteLine("                                    <i class=\"fas fa-cogs fa-sm fa-fw mr-2 text-gray-400\"></i>", "");
            f.WriteLine("                                    Settings", "");
            f.WriteLine("                                </a>", "");
            f.WriteLine("                                <a class=\"dropdown-item\" href=\"#\">", "");
            f.WriteLine("                                    <i class=\"fas fa-list fa-sm fa-fw mr-2 text-gray-400\"></i>", "");
            f.WriteLine("                                    Activity Log", "");
            f.WriteLine("                                </a>", "");
            f.WriteLine("                                <div class=\"dropdown-divider\"></div>", "");
            f.WriteLine("                                <a class=\"dropdown-item\" href=\"#\" data-toggle=\"modal\" data-target=\"#logoutModal\">", "");
            f.WriteLine("                                    <i class=\"fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400\"></i>", "");
            f.WriteLine("                                    Logout", "");
            f.WriteLine("                                </a>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                        </li>", "");
            f.WriteLine("", "");
            f.WriteLine("                    </ul>", "");
            f.WriteLine("", "");
            f.WriteLine("                </nav>", "");
            f.WriteLine("                <!-- End of Topbar -->", "");
            f.WriteLine("                <!-- Begin Page Content -->", "");
            f.WriteLine("                <div class=\"container-fluid\">", "");
            f.WriteLine("                    @RenderBody()", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("			<footer class=\"sticky-footer bg-white\">", "");
            f.WriteLine("                <div class=\"container my-auto\">", "");
            f.WriteLine("                    <div class=\"copyright text-center my-auto\">", "");
            f.WriteLine("                        <span>Copyright &copy; {0} @DateTime.Now.Year</span>", application.DisplayName);
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </footer>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("    <!-- Scroll to Top Button-->", "");
            f.WriteLine("    <a class=\"scroll-to-top rounded\" href=\"#page-top\">", "");
            f.WriteLine("        <i class=\"fas fa-angle-up\"></i>", "");
            f.WriteLine("    </a>", "");
            f.WriteLine("", "");
            f.WriteLine("    <!-- Logout Modal-->", "");
            f.WriteLine("    <div class=\"modal fade\" id=\"logoutModal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"exampleModalLabel\" aria-hidden=\"true\">", "");
            f.WriteLine("        <div class=\"modal-dialog\" role=\"document\">", "");
            f.WriteLine("            <div class=\"modal-content\">", "");
            f.WriteLine("                <div class=\"modal-header\">", "");
            f.WriteLine("                    <h5 class=\"modal-title\" id=\"exampleModalLabel\">Ready to Leave?</h5>", "");
            f.WriteLine("                    <button class=\"close\" type=\"button\" data-dismiss=\"modal\" aria-label=\"Close\">", "");
            f.WriteLine("                        <span aria-hidden=\"true\">×</span>", "");
            f.WriteLine("                    </button>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("                <div class=\"modal-body\">Select \"Logout\" below if you are ready to end your current session.</div>", "");
            f.WriteLine("                <div class=\"modal-footer\">", "");
            f.WriteLine("                    <button class=\"btn btn-secondary\" type=\"button\" data-dismiss=\"modal\">Cancel</button>", "");
            f.WriteLine("                    @using (Html.BeginForm(\"LogOff\", \"Account\", FormMethod.Post, new {{ id = \"logoutForm\", @class = \"navbar-right\" }}))", "");
            f.WriteLine("                    {{", "");
            f.WriteLine("                        @Html.AntiForgeryToken()", "");
            f.WriteLine("                        <a class=\"btn btn-primary\" href=\"javascript:document.getElementById('logoutForm').submit()\">Logout</a>", "");
            f.WriteLine("                    }}", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jquery\")", "");
            f.WriteLine("    <script src=\"~/Scripts/umd/popper.min.js\"></script>", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/bootstrap\")", "");
            f.WriteLine("    <script src=\"~/Scripts/jquery.easing.min.js\"></script>", "");
            f.WriteLine("    <script src=\"~/Scripts/sb-admin-2.min.js\"></script>", "");
            f.WriteLine("    @RenderSection(\"scripts\", required: false)", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");

        }

        private static StreamWriter CreateFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            StreamWriter _File = File.CreateText(filePath);
            return _File;
        }
    }
}
