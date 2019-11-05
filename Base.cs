using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika
{
    public class Base : IDisposable
    {
        public static string BasePath = System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "AppData").Replace("file:\\","");
        protected List<StreamWriter> _FileList = new List<StreamWriter>();

        protected void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        protected StreamWriter CreateFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            StreamWriter _File = File.CreateText(filePath);
            _FileList.Add(_File);
            return _File;
        }

        public void Dispose()
        {
            foreach (StreamWriter sw in _FileList)
            {
                if (sw != null)
                    sw.Dispose();
            }

            _FileList.Clear();
        }
    }
}
