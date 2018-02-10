using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class ResultAuditingStore_Tests : NormalizationTestBase
    {
        private readonly IResultAuditingStore _resultAuditingStore;
        public ResultAuditingStore_Tests()
        {
            _resultAuditingStore = Resolve<IResultAuditingStore>();
        }

        [Fact( DisplayName = "输出包装模块审计存储保存" )]
        public void Save_Test()
        {
            var response = new NormalizationResponse( "003", "00145534" );
            _resultAuditingStore.Save( response );
            ((MemoryListResultAuditingStore)_resultAuditingStore).ResultAuditingStore.ShouldContain( response );
        }

        [Fact( DisplayName = "输出包装模块审计存储异步保存" )]
        public async Task SaveAsync_Test()
        {
            var response = new NormalizationResponse( "003", "00145534" );
            await _resultAuditingStore.SaveAsync( response );
            ((MemoryListResultAuditingStore)_resultAuditingStore).ResultAuditingStore.ShouldContain( response );
        }
    }
}
