namespace Models
{
  public class LoginUserDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }

  public class LoginUserResponseDTO
  {
    public LoginUserDTO User { get; set; }
    public string AccessToken { get; set; }
  }
}