using System.ComponentModel.DataAnnotations;

namespace khiemnguyen_FrontEnd.Models
{
	public class UserInfo
	{



		[Key]

		public int UserId { get; set; }
		public string? FullName { get; set; }
		public string? DisplayName { get; set; }
		public string? UserName { get; set; }


		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? Role { get; set; }
		public byte[]? Image { get; set; }
		public DateTime? CreatedDate { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
		public int? quationNo { get; set; }
		public string? Answer { get; set; }
		public bool StatusAccount { get; set; } = false;
	}
}
