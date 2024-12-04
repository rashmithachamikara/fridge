using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos
{
    public class ItemDto
    {
        public string Name { get; set; } = String.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}