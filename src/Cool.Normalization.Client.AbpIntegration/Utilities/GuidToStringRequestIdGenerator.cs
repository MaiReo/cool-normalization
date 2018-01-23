
namespace Cool.Normalization.Client
{
    public class GuidToStringRequestIdGenerator : IRequestIdGenerator
    {
        public GuidToStringRequestIdGenerator()
        {
            RequestId = System.Guid.NewGuid().ToString( "N" );
        }
        public string RequestId { get; }
    }
}
