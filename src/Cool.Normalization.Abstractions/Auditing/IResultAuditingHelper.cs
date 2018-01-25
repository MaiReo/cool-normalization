using System.Reflection;
using System.Threading.Tasks;

namespace Cool.Normalization.Auditing
{
    public interface IResultAuditingHelper
    {

        bool ShouldSaveAudit( MethodInfo methodInfo, bool defaultValue = true );

        void Save( Models.NormalizationResponseBase normalizationResponse );

        Task SaveAsync( Models.NormalizationResponseBase normalizationResponse );
      
    }
}
