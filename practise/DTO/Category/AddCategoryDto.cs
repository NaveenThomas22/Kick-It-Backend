using System.ComponentModel.DataAnnotations;

namespace practise.DTO.Category
{
    public class AddCategoryDto
    {
        [Required(ErrorMessage ="Category name is required")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
        public string CategoryName { get; set; }
    }
}
