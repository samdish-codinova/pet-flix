using System.Runtime.Serialization;

namespace BusinessLogicLayer
{
  public class MovieNotFound : Exception
  {
    public MovieNotFound()
    {
    }

    public MovieNotFound(string? message) : base(message)
    {
    }

    public MovieNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected MovieNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}