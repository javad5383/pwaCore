using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PwaCore.Models
{

    public class CartDetail
    {
        [Key]
        public int DetailId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }



        [ForeignKey("ProductId ")]
        public Products Product { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }




    }
}
