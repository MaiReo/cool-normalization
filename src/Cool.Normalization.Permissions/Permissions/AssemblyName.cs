using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class AssemblyName : IAssemblyName
    {
        public AssemblyName(string uniqueName, string displayName = default( string ))
        {
            this.UniqueName = uniqueName 
                ?? throw new ArgumentNullException( nameof( uniqueName ) );
            this.DisplayName = displayName;
        }
        public string UniqueName { get; set; }

        public string DisplayName { get; set; }
    }
}
