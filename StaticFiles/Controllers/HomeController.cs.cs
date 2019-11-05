using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Controllers
{
    public class HomeController_cs
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    [Authorize]");
            f.WriteLine("    public class HomeController : Controller", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ActionResult Index()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return View();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");
        }
    }
}
