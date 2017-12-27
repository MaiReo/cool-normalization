using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerCallExpressionBuilder
    {
        Expression<Func<object, Task>> Build( Type messageType, string message );
    }
}
