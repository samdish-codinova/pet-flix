using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
  public class Rental
  {
    public int Id { set; get; }
    public required int MovieId { get; set; }
    public Movie Movie { get; set; }
    public required int UserId { get; set; }
    public User User { get; set; }
    public required DateTime IssuedDate { get; set; }
    public DateTime? ReturnDate { get; set; } = null;
    public required decimal TotalPrice { get; set; }
  }
}