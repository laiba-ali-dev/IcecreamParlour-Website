using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class Userregister
    {

        [Key]
        public int RegisterId { get; set; }

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

        [Required]
        public string SubscriptionType { get; set; } // Monthly or Yearly

        [Required]
        public string PaymentStatus { get; set; } // Paid or Unpaid

       
        public DateTime? PaymentDate { get; set; }

      
        public DateTime? ExpiryDate { get; set; }


    }
}
