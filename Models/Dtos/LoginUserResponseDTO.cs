namespace Models
{
  public class LoginUserResponseDTO
  {
    public required BaseUser User { get; set; }
    public required string AccessToken { get; set; }
  }
}