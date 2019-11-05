using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Converters
{
    public class DateTimeConverter_cs
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using Newtonsoft.Json.Converters;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Converters", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class CustomDateTimeSecondConverter : IsoDateTimeConverter", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public CustomDateTimeSecondConverter()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            base.DateTimeFormat = \"MM/dd/yyyy hh:mm:ss\";", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("", "");
            f.WriteLine("    public class CustomDateTimeConverter : IsoDateTimeConverter", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public CustomDateTimeConverter()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            base.DateTimeFormat = \"MM/dd/yyyy hh:mm\";", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("", "");
            f.WriteLine("    public class CustomDateConverter : IsoDateTimeConverter", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public CustomDateConverter()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            base.DateTimeFormat = \"MM/dd/yyyy\";", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");
        }
    }
}
