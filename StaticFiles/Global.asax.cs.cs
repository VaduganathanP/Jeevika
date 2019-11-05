using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Global_asax_cs
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("using System.Web.Optimization;", "");
            f.WriteLine("using System.Web.Routing;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class MvcApplication : System.Web.HttpApplication", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        protected void Application_Start()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            AreaRegistration.RegisterAllAreas();", "");
            f.WriteLine("            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);", "");
            f.WriteLine("            RouteConfig.RegisterRoutes(RouteTable.Routes);", "");
            f.WriteLine("            BundleConfig.RegisterBundles(BundleTable.Bundles);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

        }
    }
}
