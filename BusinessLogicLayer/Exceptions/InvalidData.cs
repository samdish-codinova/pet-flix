using System.Runtime.Serialization;

namespace BusinessLogicLayer
{
  public class InvalidData : Exception
  {
    public InvalidData()
    {
    }

    public InvalidData(string? message) : base(message)
    {
    }

    public InvalidData(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidData(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}