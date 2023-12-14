using System.ComponentModel.DataAnnotations;

namespace khiemnguyen.WebApi.Models
{
    public class Tag_Include
    {
        [Key]
        public int id { get; set; }
        public int Tagid  { get; set; }
        public int Menuid { get; set; }

    }
}
