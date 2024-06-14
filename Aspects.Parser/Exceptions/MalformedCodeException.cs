namespace Aspects.Parsers.CSharp.Exceptions
{
    public class MalformedCodeException : Exception
    {
        public MalformedCodeException(string message, Exception innerException)
            : base(message, innerException) 
        { }

        public MalformedCodeException(string message)
            : base(message) 
        { }
    }
}
