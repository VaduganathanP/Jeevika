using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles.App_Start
{
    public class BundleConfig
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Optimization;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class BundleConfig", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862", "");
            f.WriteLine("        public static void RegisterBundles(BundleCollection bundles)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            bundles.Add(new ScriptBundle(\"~/bundles/jquery\").Include(", "");
            f.WriteLine("                        \"~/Scripts/jquery-{{version}}.js\"));", "");
            f.WriteLine("", "");
            f.WriteLine("            bundles.Add(new ScriptBundle(\"~/bundles/jqueryval\").Include(", "");
            f.WriteLine("                        \"~/Scripts/jquery.validate*\"));", "");
            f.WriteLine("", "");
            f.WriteLine("            // Use the development version of Modernizr to develop with and learn from. Then, when you're", "");
            f.WriteLine("            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.", "");
            f.WriteLine("            bundles.Add(new ScriptBundle(\"~/bundles/modernizr\").Include(", "");
            f.WriteLine("                        \"~/Scripts/modernizr-*\"));", "");
            f.WriteLine("", "");
            f.WriteLine("            bundles.Add(new ScriptBundle(\"~/bundles/bootstrap\").Include(", "");
            f.WriteLine("                      \"~/Scripts/bootstrap.js\"));", "");
            f.WriteLine("", "");
            f.WriteLine("            bundles.Add(new StyleBundle(\"~/Content/css\").Include(", "");
            f.WriteLine("                      \"~/Content/bootstrap.css\",", "");
            f.WriteLine("                      \"~/Content/site.css\"));", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

        }
    }
}
