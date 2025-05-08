using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class Feedback
    {
        [Key]

        public int FeedbackId { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        public string Email { get; set; }


        [Required]
        public string Feedabck { get; set; }

        

    }
}
