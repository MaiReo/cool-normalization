using Abp.Dependency;
using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerCodeResolver : IMessageHandlerCodeResolver, ISingletonDependency
    {
        public IReadOnlyDictionary<CodePart, string> ResolveCode(
            MethodInfo ifaceMethod,
            MethodInfo implMethod, Exception exception = null)
        {

            implMethod = implMethod?.GetBaseDefinition();

            var methodCodes = implMethod?.GetCustomAttributes()?.OfType<ICodeAttribute>();

            var classCodes = implMethod?.DeclaringType?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();

            var ifaceMethodCodes = ifaceMethod?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var ifaceCodes = ifaceMethod?.DeclaringType?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();

            var codes = CodeAttribute.GenerateCodesForSuccess( methodCodes, classCodes, ifaceMethodCodes, ifaceCodes,
                exception == null ? Codes.Level.Success : Codes.Level.Fatal );
            if (exception != null)
            {
                codes = CodeAttribute.GenerateCodesForError( codes, exception );
            }
            return codes;
        }
    }
}
