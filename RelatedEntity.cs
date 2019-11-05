using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevika
{
    public class RelatedEntity
    {
        public string Name { get; set; }
        public Entity Entity { get; set; }
        public string DisplayName { get; set; }
        public bool IsVisibleInListingScreen { get; set; } = true;
        public bool IsRequired { get; set; } = false;
        public bool ShowInTab { get; set; } = true;
        public bool CascadeOnDelete { get; set; } = true;
        public bool IsHidden { get; set; } = false;
        public string DefaultValue { get; set; }
    }
}
