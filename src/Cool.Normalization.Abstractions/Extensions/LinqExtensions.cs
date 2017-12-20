using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Do NOT use the null conditional operator when calling this method. (?.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConcatSkipNullOrEmpty<T>( this IEnumerable<T> source, IEnumerable<T> second )
        {
            if (source?.Any() != true) return second;
            if (second?.Any() != true) return source;
            return source.Concat( second );
        }
    }
}
