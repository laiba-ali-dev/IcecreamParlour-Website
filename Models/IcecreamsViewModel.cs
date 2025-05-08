using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class IcecreamsViewModel
    {
        [Key]
        public int IcecreamId { get; set; }

        [Required]
        public string Icecreamname { get; set; }

        [Required]
        public string Icecreamprice { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
