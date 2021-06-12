using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        [NotMapped]
        public virtual IFormFile ImgFile { get; set; }
    }
}
