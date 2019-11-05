using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.App_Start
{
    public class FilterConfig
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class FilterConfig", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public static void RegisterGlobalFilters(GlobalFilterCollection filters)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            filters.Add(new HandleErrorAttribute());", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");
        }
    }
}
