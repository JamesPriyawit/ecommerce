using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
