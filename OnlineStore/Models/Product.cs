// Product.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStore;

namespace OnlineStore.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        public byte[] Image { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual ProductCategory Category { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
