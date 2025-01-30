namespace Models
{
  public enum UserRole {
    Admin,
    Customer
  }
  public class User
  {
    public int Id { set; get; }
    public required string Name { set; get; }
    public required string Email { set; get; }
    public required string PasswordSalt { set; get; }
    public required string PasswordHash { set; get; }
    public UserRole Role { set; get; }
    public DateTime? CreatedAt { set; get; }
    public DateTime? UpdatedAt { set; get; }
  }
}