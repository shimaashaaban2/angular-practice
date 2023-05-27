using System.ComponentModel.DataAnnotations;

namespace movie_crud.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }

        public double Rate { get; set; }

        [Required, MaxLength(2500)]
        public string Storyline { get; set; }
        [Required]
        public byte[] ?Poster { get; set; }

        public byte GenereId { get; set; }

        public Genere Genere{ get; set; }

    }
}
