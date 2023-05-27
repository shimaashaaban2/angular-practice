using movie_crud.Models;
using System.ComponentModel.DataAnnotations;

namespace movie_crud.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2500)]
        public string Storyline { get; set; }
        [Display(Name = "Select Poster...")] 
        public byte[] Poster { get; set; }

        [Display(Name = "Genere")]
        public byte GenreId { get; set; }

        public IEnumerable<Genere> Generes{ get; set; }



    }
}
