using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.App_Start
{
    public class RouteConfig
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("using System.Web.Routing;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class RouteConfig", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public static void RegisterRoutes(RouteCollection routes)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            routes.IgnoreRoute(\"{{resource}}.axd/{{*pathInfo}}\");", "");
            f.WriteLine("", "");
            f.WriteLine("            routes.MapRoute(", "");
            f.WriteLine("                name: \"Default\",", "");
            f.WriteLine("                url: \"{{controller}}/{{action}}/{{id}}\",", "");
            f.WriteLine("                defaults: new {{ controller = \"Home\", action = \"Index\", id = UrlParameter.Optional }}", "");
            f.WriteLine("            );", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

        }
    }
}
