using System.ComponentModel.DataAnnotations;

namespace khiemnguyen.WebApi.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }
}
