using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public interface  IAssemblyName
    {
        string UniqueName { get; set; }

        string DisplayName { get; set; }
    }
}
