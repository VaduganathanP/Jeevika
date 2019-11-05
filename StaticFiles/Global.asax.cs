using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Global_asax
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<%@ Application Codebehind=\"Global.asax.cs\" Inherits=\"{0}.MvcApplication\" Language=\"C#\" %>", application.Name);
        }
    }
}
