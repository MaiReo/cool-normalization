using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cool.Normalization.Messages.Tests
{
    internal static class ObjectExtensions
    {
        public static string ToJson<T>(this T @this)
            where T : class =>
            @this == default( T )
            ? "{}"
            : JsonConvert.SerializeObject( @this );
    }
}
