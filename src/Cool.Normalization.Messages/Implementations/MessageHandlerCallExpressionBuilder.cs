using MaiReo.Messages.Abstractions;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerCallExpressionBuilder : IMessageHandlerCallExpressionBuilder
    {
        public Expression<Func<object, Task>> Build(Type messageType, IMessageWrapper wrapper)
        {
            var handlerType = typeof( IMessageHandler<> ).MakeGenericType( messageType );
            var objectParameter = Expression.Parameter( typeof( object ) );
            var jsonExpr = Expression.Constant( wrapper.Message, typeof( string ) );
            var handlerInstanceExpr = Expression.Convert( objectParameter, handlerType );
            var deserializeCall = Expression.Call(
                typeof( JsonConvert ),
                nameof( JsonConvert.DeserializeObject ),
                new[] { messageType }, jsonExpr );
            var convertJsonToMessageTypeExpr = Expression.Convert( deserializeCall, messageType );

            var timestampExpr = Expression.Constant( wrapper.Timestamp, typeof( DateTimeOffset ) );

            var bodyExpr = Expression.Call( handlerInstanceExpr,
                nameof( IMessageHandler<DummyMessage>.HandleMessageAsync ),
                Type.EmptyTypes,
                convertJsonToMessageTypeExpr,
                timestampExpr );
            var lambdaExpr = Expression.Lambda<Func<object, Task>>( bodyExpr, objectParameter );
            return lambdaExpr;
        }
        private class DummyMessage : IMessage
        {
        }
    }
}