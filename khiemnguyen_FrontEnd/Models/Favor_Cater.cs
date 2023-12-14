using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace khiemnguyen_FrontEnd.Models
{
	public class Favor_Cater
	{
		[Key]
		public int id { get; set; }
		public int Custid { get; set; }
		public int Caterid { get; set; }
        [NotMapped]
        public  string name { get; set; }
    }
}
