using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen_FrontEnd.Models
{
	public class Food_in_Menu
	{
		public int id { get; set; }
		public int Menuid { get; set; }
		public int Foodid { get; set; }

		[NotMapped]
		public string? Name { get; set; }
        [NotMapped]
        public byte[]? image { get; set; }
    }
}
