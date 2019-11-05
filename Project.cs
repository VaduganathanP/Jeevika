using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jeevika
{
    public class Project : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Application> Applications { get; set; } = new List<Application>();

        public Application AddApplication(Application application)
        {
            Applications.Add(application);
            return application;
        }

        public void Generate(string projectId = null)
        {
            if(projectId != null)
            {
                BasePath = Path.Combine(BasePath, projectId);
            }
            var projectDir = Path.Combine(BasePath, Name);
            base.CreateDirectory(projectDir);

            foreach (Application application in Applications)
            {
                application.Generate(projectDir);
            }
        }

    }
}
