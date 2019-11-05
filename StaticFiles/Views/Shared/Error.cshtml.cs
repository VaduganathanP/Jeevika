using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.Views.Shared
{
    public class Error_cshtml
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width\" />", "");
            f.WriteLine("    <title>Error</title>", "");
            f.WriteLine("</head>", "");
            f.WriteLine("<body>", "");
            f.WriteLine("    <hgroup>", "");
            f.WriteLine("        <h1>Error.</h1>", "");
            f.WriteLine("        <h2>An error occurred while processing your request.</h2>", "");
            f.WriteLine("    </hgroup>", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");
        }
    }
}
