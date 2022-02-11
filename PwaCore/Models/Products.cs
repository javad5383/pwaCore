using System.ComponentModel.DataAnnotations;

namespace PwaCore.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public string? MainImg { get; set; }

        public  bool ForMan { get; set; }
        public  bool ForWoMan { get; set; }
        public bool ForChildren { get; set; } 

        public List<ProductImages>? ProductImages { get; set; }



    }
}
