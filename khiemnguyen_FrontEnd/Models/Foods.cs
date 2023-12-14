namespace khiemnguyen_FrontEnd.Models
{
	public class Foods

	{
		public int id { get; set; }
		public int Caterid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }

		public byte[]? Image { get; set; }
	}
}
