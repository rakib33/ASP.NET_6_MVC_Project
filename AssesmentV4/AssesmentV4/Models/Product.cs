using System;
using System.ComponentModel.DataAnnotations;

namespace AssesmentV4.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string Price { get; set; }
        public string DiscountedPrice { get; set; }
    }
}
