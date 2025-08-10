using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace loginlogout.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }  

        public IFormFile ProductPhoto { get; set; } // For uploading image

        public string LongDescription { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }
      

        // Age-wise Stock Inputs from Form
       
    }
}
