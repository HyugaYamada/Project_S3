using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen.WebApi.Models
{
	public class Cart
	{
		public int menuid { get; set; }
		public int? custid { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }

		
	}
}
