using System.ComponentModel.DataAnnotations;

namespace khiemnguyen.WebApi.Models
{
    public class Menu_Tag
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
