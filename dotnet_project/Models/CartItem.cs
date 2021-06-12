using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Product Product { get; set; }
    }
}
