using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public static class ReflectionExtensions
    {
        /// <summary>
        ///  Thanks to https://stackoverflow.com/questions/22598323/movenext-instead-of-actual-method-task-name
        ///  The author said DO NOT use in Production,
        ///  but I have no other ideas to detect real method.
        ///  Also I think we just use this to trace and locate the exception,
        ///  we will fix the bug using a little time when we found it.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static MethodBase
            GetRealMethodFromAsyncMethodOrSelf( this MethodBase method )
        {
            if (method == null)
            {
                return method;
            }
            var generatedType = method.DeclaringType;
            var originalType = generatedType.DeclaringType;
            if (originalType == null)
            {
                return method;
            }
            //Async method detection.
            try
            {
                var matchingMethods =
                from methodInfo in originalType
                .GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy )
                let attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>()
                where attr != null && attr.StateMachineType == generatedType
                select methodInfo;
                return matchingMethods.Single();
            }
            catch
            {
            }
            finally
            {
            }
            return method;
        }
    }
}
