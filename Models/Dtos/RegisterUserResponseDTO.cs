namespace Models
{
  public class RegisterUserResponseDTO
  {
    public BaseUser User { get; set; }
    public string AccessToken { get; set; }
  }
}