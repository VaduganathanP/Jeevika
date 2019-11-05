using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class Property : Base
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public eVariableType VariableType { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; } = false;
        public bool IsVisibleInListingScreen { get; set; } = false;
        public string DefaultValue { get; set; }
    }
}
