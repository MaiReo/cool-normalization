using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class CoolPermission
    {
        public CoolPermission(string name, string displayName)
        {
            this.Name = name;
            this.DisplayName = displayName;
        }
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}
