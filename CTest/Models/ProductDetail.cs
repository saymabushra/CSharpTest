using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTest.Models
{
    public class ProductDetail
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Availability { get; set; }
        public List<Specs> Specs { get; set; }
    }
}