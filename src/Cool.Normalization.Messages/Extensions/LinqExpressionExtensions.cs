using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System.Linq.Expressions.Extensions
{
    public static class LinqExpressionExtensions
    {
        public static MethodInfo AsMethod(this Expression expression)
        {
            if (expression == null)
                return null;
            var body = expression;
            if (body.NodeType == ExpressionType.Lambda)
                body = (body as LambdaExpression).Body;
            while (body.NodeType == ExpressionType.Convert)
                body = (body as UnaryExpression).Operand;
            if (body.NodeType == ExpressionType.Call)
            {
                return (body as MethodCallExpression).Method;
            }
            return null;
        }
    }
}
