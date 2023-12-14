using System.ComponentModel.DataAnnotations;

namespace khiemnguyen_FrontEnd.Models
{
	public class Category
	{
		[Key]
		public int id { get; set; }
		public string Name { get; set; }
	}
}
