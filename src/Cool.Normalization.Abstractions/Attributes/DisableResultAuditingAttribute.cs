using System;

namespace Cool.Normalization
{
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false )]
    public class DisableResultAuditingAttribute : Attribute
    {
    }
}
