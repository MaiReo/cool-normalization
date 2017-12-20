using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    [AttributeUsage( AttributeTargets.Interface 
        | AttributeTargets.Class 
        | AttributeTargets.Method 
        | AttributeTargets.Property,
        Inherited = true, 
        AllowMultiple = true )]
    public class CodeAttribute : Attribute, ICodeAttribute
    {
        public CodeAttribute( string code )
        {
            this.Code = code;
            CodePart = CodePart.Default;
        }

        public virtual CodePart CodePart { get; set; }

        public virtual string Code { get; set; }


    }
}
