using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen_FrontEnd.Models
{
	public class FeedBack
	{
		[Key]
		public int id { get; set; }
		public int Menuid { get; set; }
		public int Userid { get; set; }
		public string Replyto { get; set; }
		public string Message { get; set; }
		public byte[]? image { get; set; }
		public DateTime Timestamp { get; set; }
		public int rate { get; set; }
		public int orderid { get; set; }
		[NotMapped]
		public string UserName { get; set; }
	}
}
