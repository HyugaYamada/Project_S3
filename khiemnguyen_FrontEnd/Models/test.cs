using System.ComponentModel.DataAnnotations;

namespace khiemnguyen_FrontEnd.Models
{
	public class test
	{
		[Key]
		public int id { get; set; }
		public string? Name { get; set; }
		public int Menuid { get; set; }
		public float Quantity { get; set; }
		public float TotalPrice { get; set; }
		public string? Description { get; set; }
		public int Caterid { get; set; }
		public int? Custid { get; set; }
		public int? GuestNum { get; set; }
		public DateTime? DelivDate { get; set; }
		public DateTime? OrdDate { get; set; }
		public string? Status { get; set; }

	}
}
