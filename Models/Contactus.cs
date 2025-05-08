using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class Contactus
    {

        [Key]
        public int Contactusid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

      

    }
}
