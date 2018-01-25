using Cool.Normalization;
using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ExceptionExtensions
    {

        public static NormalizationException WithCode(
            this Exception exception,
            string detail = Codes.Detail.Default,
            string errorLevel = Codes.Level.Fatal,
            string message = default( string ))
        {
            return NormalizationException.WithCode( detail,
                errorLevel, message, exception );
        }


    }
}
