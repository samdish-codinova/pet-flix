namespace Models
{
  public class LoginUserRequestDTO
  {
    public required string Email { get; set; }
    public required string Password { get; set; }
  }
}