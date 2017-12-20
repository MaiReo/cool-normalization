namespace Cool.Normalization
{
    /// <summary>
    /// This interface defines Code using by wrapping output result.
    /// Implementation class(es) must be derived from <see cref="System.Attribute"/>
    /// </summary>
    public interface ICodeAttribute
    {
        CodePart CodePart { get; }

        string Code { get; }
    }
}
