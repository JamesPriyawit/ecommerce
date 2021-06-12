using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string ApplicationUserId { get; set; }
        public double TotleAmount { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
