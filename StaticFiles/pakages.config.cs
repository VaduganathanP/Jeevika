﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika.StaticFiles
{
    public class Packages_config
    {
        public static void WriteContent(StreamWriter f, Application application)
        {
            f.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            f.WriteLine("<packages>", "");
            f.WriteLine("  <package id=\"Antlr\" version=\"3.5.0.2\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"bootstrap\" version=\"4.3.1\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"jQuery\" version=\"3.4.1\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"jQuery.Validation\" version=\"1.17.0\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.AspNet.Mvc\" version=\"5.2.7\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.AspNet.Razor\" version=\"3.2.7\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.AspNet.Web.Optimization\" version=\"1.1.3\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.AspNet.WebPages\" version=\"3.2.7\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.CodeDom.Providers.DotNetCompilerPlatform\" version=\"2.0.1\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.jQuery.Unobtrusive.Validation\" version=\"3.2.11\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Microsoft.Web.Infrastructure\" version=\"1.0.0.0\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Modernizr\" version=\"2.8.3\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"Newtonsoft.Json\" version=\"12.0.2\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"popper.js\" version=\"1.14.3\" targetFramework=\"net472\" />", "");
            f.WriteLine("  <package id=\"WebGrease\" version=\"1.6.0\" targetFramework=\"net472\" />", "");
            f.WriteLine("</packages>", "");
        }
    }
}
