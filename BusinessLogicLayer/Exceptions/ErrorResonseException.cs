using System.Net;

namespace BusinessLogicLayer
{
  public class ErrorResponseException : Exception
  {
    public HttpStatusCode StatusCode { get; }

    public ErrorResponseException(string message, HttpStatusCode statusCode) : base(message)
    {
      StatusCode = statusCode;
    }
  }
}