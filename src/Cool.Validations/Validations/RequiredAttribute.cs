using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cool.Validations
{
    /// <summary>
    /// 验证属性是必填项
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        /// <summary>
        /// 允许类型默认值。
        /// int => 0,
        /// long => 0L,
        /// 等等。
        /// 默认值: false
        /// </summary>
        public bool AllowDefaultValue { get; set; }
        /// <summary>
        /// 验证属性是必填项，不允许类型默认值
        /// </summary>
        public RequiredAttribute()
        {
            AllowEmptyStrings = false;
        }

        public override bool IsValid(object value)
        {
            //Reference type pass to base class to check.
            var isValid = base.IsValid( value );
            if (!isValid)
            {
                return false;
            }
            var type = value?.GetType() ?? typeof( object );

            var default_expression = Expression.Default( type );
            var value_expression = Expression.Constant( value, type );
            var body_expression = Expression.Equal( default_expression, value_expression );
            var lambda = Expression.Lambda<Func<bool>>( body_expression );
            var @delegate = lambda.Compile();

            var isDefault = @delegate();
            if (isDefault)
            {
                return AllowDefaultValue;
            }
            return true;
        }
    }
}
