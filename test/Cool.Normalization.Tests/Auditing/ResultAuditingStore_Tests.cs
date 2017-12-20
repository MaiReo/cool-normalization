using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class ResultAuditingStore_Tests : MicroServicesnormalizationTestBase
    {
        private readonly IResultAuditingStore _resultAuditingStore;
        public ResultAuditingStore_Tests()
        {
            _resultAuditingStore = Resolve<IResultAuditingStore>();
        }

        [Fact]
        public void Save_Test()
        {
            var response = new NormalizationResponse( "003", "00145534" );
            _resultAuditingStore.Save( response );
            ((MemoryListResultAuditingStore)_resultAuditingStore).ResultAuditingStore.ShouldContain( response );
        }

        [Fact]
        public async Task SaveAsync_Test()
        {
            var response = new NormalizationResponse( "003", "00145534" );
            await _resultAuditingStore.SaveAsync( response );
            ((MemoryListResultAuditingStore)_resultAuditingStore).ResultAuditingStore.ShouldContain( response );
        }
    }
}
