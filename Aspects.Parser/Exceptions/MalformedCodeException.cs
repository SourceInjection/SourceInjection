
namespace Aspects.Parsers.CSharp.Exceptions
{
    [Serializable]
    public class MalformedCodeException(string message, Exception innerException) 
        : Exception(message, innerException)
    { }
}
