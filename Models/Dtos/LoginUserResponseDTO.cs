namespace Models
{
  public class LoginUserResponseDTO
  {
    public BaseUser User { get; set; }
    public string AccessToken { get; set; }
  }
}