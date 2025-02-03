namespace Models
{
  public class RentalResponseDTO
  {
    public int Id { set; get; }
    public Movie Movie { get; set; }
    public BaseUser User { get; set; }
    public required DateTime IssuedDate { get; set; }
    public DateTime? ReturnDate { get; set; } = null;
    public required decimal TotalPrice { get; set; }
  }
}