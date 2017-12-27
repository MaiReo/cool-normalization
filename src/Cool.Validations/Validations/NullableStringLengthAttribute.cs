using System;
using System.ComponentModel.DataAnnotations;

namespace Cool.Validations
{
    [AttributeUsage( AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
    public class NullableStringLengthAttribute : StringLengthAttribute
    {
        /// <summary>
        /// 允许空引用字符串
        /// 默认值:true
        /// </summary>
        public bool AllowNullString { get; set; }
        /// <summary>
        /// 允许空长度字符串
        /// 默认值:false
        /// </summary>
        public bool AllowEmptyString { get; set; }

        public NullableStringLengthAttribute( int maximumLength ) : base( maximumLength )
        {
            AllowNullString = true;
        }

        public override bool IsValid( object value )
        {
            if (value == default( object ) || object.ReferenceEquals( value, default( object ) ))
            {
                return AllowNullString;
            }
            if (value is string str)
            {
                if (str == default( string ) || object.ReferenceEquals( str, default( string ) ))
                {
                    return AllowNullString;
                }
                if (str.Trim() == string.Empty)
                {
                    return AllowEmptyString;
                }
            }
            return base.IsValid( value );
        }
    }
}
