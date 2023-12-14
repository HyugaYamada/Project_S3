using System.ComponentModel.DataAnnotations;

namespace khiemnguyen_FrontEnd.Models
{
    public class Menu
    {

        [Key]
        public int id { get; set; }
        public int CaterID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
		public int israted { get; set; }
        public string? Category { get; set; }
    }
}
