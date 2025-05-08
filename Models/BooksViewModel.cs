using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class BooksViewModel
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Booktittle { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string Stock { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public bool IsAvailable { get; set; } // true = available, false = unavailable

        [Required]
        public string CreatedDate { get; set; }

    }
}
