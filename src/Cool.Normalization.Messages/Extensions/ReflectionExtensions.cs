using System.Linq;

namespace System.Reflection.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodInfo ImplementationAt(
            this MethodInfo method, object instance)
        {
            if (method == null)
                return null;
            var instanceType = instance?.GetType();
            if (instanceType == null)
            {
                return null;
            }
            if (!method.DeclaringType.IsAssignableFrom( instanceType ))
            {
                return null;
            }
            var instanceMethod = instanceType.GetMethod( method.Name,
                method.GetParameters()
                .Select( p => p.ParameterType )
                .ToArray() );
            return instanceMethod;
        }
    }
}
