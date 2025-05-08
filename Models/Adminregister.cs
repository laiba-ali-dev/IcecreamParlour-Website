using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class Adminregister
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
