namespace Cool.Normalization.Models
{
    public class NormalizationResponse<T> : NormalizationResponseBase where T : class, new()
    {
        public T Data { get; set; }

        public NormalizationResponse()
        {

        }

        public NormalizationResponse( string requestId, string code, T data = default( T ) ) : base( requestId, code )
        {
            this.Data = data;
        }
    }
}