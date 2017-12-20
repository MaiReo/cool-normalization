using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Utilities
{
    public interface IErrorMessageGenerator
    {
        string GetErrorMessage( Exception exception );
    }
}
