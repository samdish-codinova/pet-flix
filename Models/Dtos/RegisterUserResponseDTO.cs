namespace Models
{
  public class RegisterUserResponseDTO
  {
    public required BaseUser User { get; set; }
    public required string AccessToken { get; set; }
  }
}