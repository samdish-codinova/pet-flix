using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RentalPrice { get; set; }
        public int Stock { get; set; } = 0;
    }
}