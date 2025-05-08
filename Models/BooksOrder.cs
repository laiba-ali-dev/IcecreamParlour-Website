using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class BooksOrder
    {

        [Key]
        public int BooksOrderId { get; set; }

        [Required]
        public string Booktitle { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string TotalAmount { get; set; }

        [Required]
        public string Paying { get; set; }


        [Required]
        public string Status { get; set; }
    }
}
