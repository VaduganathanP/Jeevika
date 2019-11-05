using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Views.Home
{
    public class Index_cshtml
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("@{{", "");
            f.WriteLine("    ViewBag.Title = \"Home\";", "");
            f.WriteLine("}}", "");
        }
    }
}
