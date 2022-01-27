using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PwaCore.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public bool IsFinally { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserId { get; set; }




       [ForeignKey("UserId")]
        public Users User { get; set; }
    
        public ICollection<CartDetail> CartDetails { get; set; }

    }

    
}
