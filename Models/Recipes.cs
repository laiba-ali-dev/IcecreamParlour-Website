using System.ComponentModel.DataAnnotations;

namespace ice_cream.Models
{
    public class Recipes
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
        public string Image { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

    }
}
