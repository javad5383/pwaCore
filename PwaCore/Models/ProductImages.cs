using System.ComponentModel.DataAnnotations;

namespace PwaCore.Models
{
    public class ProductImages
    {
        [Key]
        public int ImgId { get; set; }
        public int ProductId { get; set; }
        public string? ImgName { get; set; }
        public string? ImgAlt  { get; set; }

        public virtual Products Product { get; set; }



    }
}
