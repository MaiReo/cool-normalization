namespace Cool.Normalization.Models
{
    public class NormalizationResponse : NormalizationResponseBase
    {
        public object Data { get; }


        public NormalizationResponse( string requestId, string code, object data = default(object) ) : base( requestId, code )
        {
            this.Data = data;
        }
    }
}