using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Shouldly;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class ResultAuditingHelper_Tests: MicroServicesnormalizationTestBase
    {
        private readonly IResultAuditingHelper _resultAuditingHelper;
        public ResultAuditingHelper_Tests()
        {
            _resultAuditingHelper = Resolve<IResultAuditingHelper>();
        }

        [Fact]
        public void ShouldSaveAudit_Test()
        {
            var methodInfo = new TestClass().TestMethod();
            var methodInfo2 = new TestClass().TestMethod2();
            var methodInfo3 = new TestClass().TestMethod3();
            var shouldNotSaveAudit = _resultAuditingHelper.ShouldSaveAudit( methodInfo, false );
            var shouldSaveAudit = _resultAuditingHelper.ShouldSaveAudit( methodInfo, true );
            var shouldSaveAudit2 = _resultAuditingHelper.ShouldSaveAudit( methodInfo3, false );
            var shouldNotSaveAudit2 = _resultAuditingHelper.ShouldSaveAudit( methodInfo2, true );
            
            shouldNotSaveAudit.ShouldBeFalse();
            shouldSaveAudit.ShouldBeTrue();
            shouldSaveAudit2.ShouldBeTrue();
            shouldNotSaveAudit2.ShouldBeFalse();

        }

        [Fact]
        public void Save_Test()
        {
            var store = Resolve<MemoryListResultAuditingStore>();

            var response = new NormalizationResponse( "001", "00112233" );

            _resultAuditingHelper.Save( response );

            store.ResultAuditingStore.ShouldContain(response);
        }

        [Fact]
        public async Task SaveAsync_Test()
        {
            var store = Resolve<MemoryListResultAuditingStore>();

            var response = new NormalizationResponse( "002", "00122334" );

            await _resultAuditingHelper.SaveAsync( response );

            store.ResultAuditingStore.ShouldContain( response );
        }

        class TestClass
        {
            public MethodInfo TestMethod()
            {
                return typeof( TestClass ).GetMethod( nameof( TestMethod ) );
            }

            [DisableResultAuditing]
            public MethodInfo TestMethod2()
            {
                return typeof( TestClass ).GetMethod( nameof( TestMethod2 ) );
            }
            [ResultAudited]
            public MethodInfo TestMethod3()
            {
                return typeof( TestClass ).GetMethod( nameof( TestMethod3 ) );
            }
        }
    }
}
