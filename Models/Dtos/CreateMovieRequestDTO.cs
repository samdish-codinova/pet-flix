namespace Models
{
  public class CreateMovieRequestDTO
  {
    public string Title { get; set; }

    public string Description { get; set; }

    public string Genre { get; set; }

    public DateTime ReleaseDate { get; set; }

    public decimal RentalPrice { get; set; }
    public int Stock { get; set; } = 0;
  }
}