using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class RecipesViewModel
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        public string Recipename { get; set; }

        [Required]
        public string Recipedescription { get; set; }

        [Required]
        public string Recipeingredients { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}
