namespace Models
{
  public class BaseUser
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}