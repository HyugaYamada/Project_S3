using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen_FrontEnd.Models
{
	public class Order
	{
		[Key]
		public int id { get; set; }
		public string? Name { get; set; }
		public int Menuid { get; set; }
		public float Quantity { get; set; }
		public float TotalPrice { get; set; }
		public string?	 Description { get; set; }
		public int Caterid { get; set; }
		public int? Custid { get; set; }
		public int? GuestNum { get; set; }
		public DateTime? DelivDate { get; set; }
		public DateTime? OrdDate { get; set; }
		public string? Status { get; set; }
        [NotMapped]
        public byte[]? Image { get; set; }
		public int? israted { get; set; }
        [NotMapped]
        public string MenuName { get; set; }

		[DefaultValue("0")]
		public int IsCanceled { get; set; }

        public string Venue { get; set; }
        [NotMapped]
        public string? CustomerName { get; set; }
    }
}
