using MaiReo.Messages.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerCallExpressionBuilder
    {
        Expression<Func<object, Task>> Build( Type messageType, IMessageWrapper wrapper );
    }
}
