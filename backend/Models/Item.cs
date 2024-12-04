using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}