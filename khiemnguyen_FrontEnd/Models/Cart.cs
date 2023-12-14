using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen_FrontEnd.Models
{
	public class Cart
	{
        public int menuid { get; set; }
        public int? custid { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public byte[]? Image { get; set; }
        public string MenuName { get; set; }
        [NotMapped]
        public int caterid { get; set; }
        public string Venue { get; set; }
        public DateTime dob { get; set; }
    }
}
