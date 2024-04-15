using System.ComponentModel.DataAnnotations;

namespace StackOverFlowClone.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
