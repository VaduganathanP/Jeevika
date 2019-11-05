using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class PropertyGroup : Base
    {
        public string Name { get; set; }
        public int Order { get; set; }
        List<Property> Properties { get; set; } = new List<Property>();
        public Property AddProperty(Property property)
        {
            Properties.Add(property);
            return property;
        }
    }
}
