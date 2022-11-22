using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ViewModel
{
    public class ProductViewModel
    {
        //public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> OtherImages { get; set; }
    }
}
