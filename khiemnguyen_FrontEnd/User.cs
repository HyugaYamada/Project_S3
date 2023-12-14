namespace khiemnguyen_FrontEnd
{
    public class User
    {
        public int UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }
		public string? Address { get; set; }
		public string? PhoneNo { get; set; }
		public bool StatusAccount { get; set; } = false;
	}
}
