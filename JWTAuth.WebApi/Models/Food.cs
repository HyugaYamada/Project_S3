using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace khiemnguyen.WebApi.Models
{
    public class Food
    {
        public int id { get; set; }
        public int Caterid { get; set; }
        public string Name { get; set; }
        public string   Description { get; set; }
        public string Category { get; set; }
        
        public byte[]? Image { get; set; }

    }
}
