using MaiReo.Messages.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public class NullMessageHandlerCallExpressionBuilder : IMessageHandlerCallExpressionBuilder
    {
        public Expression<Func<object, Task>> Build(Type messageType, IMessageWrapper wrapper)
            => Expression.Lambda<Func<object, Task>>(
                Expression.Constant( Task.CompletedTask, typeof( Task ) )
                , Expression.Parameter( typeof( object ) ));

        public static NullMessageHandlerCallExpressionBuilder Instance
            => new NullMessageHandlerCallExpressionBuilder();
    }
}
