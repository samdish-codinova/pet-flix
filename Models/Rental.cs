using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
  public class Rental
  {
    public required int Id { set; get; }
    public required int MovieId { get; set; }
    public required Movie Movie { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }
    public required DateTime IssuedDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public required decimal TotalPrice { get; set; }
  }
}