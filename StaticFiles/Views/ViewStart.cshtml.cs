using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Views
{
    public class ViewStart_cshtml
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("@{{", "");
            f.WriteLine("    Layout = \"~/Views/Shared/_Layout.cshtml\";", "");
            f.WriteLine("}}", "");
        }
    }
}
