using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Mappers
{
    public static class ItemMapper
    {
        public static ItemDto ToDto(this Item item)
        {
            return new ItemDto
            {
                Name = item.Name,
                ExpiryDate = item.ExpiryDate
            };
        }

        public static Item ToModel(this ItemDto itemDto)
        {
            return new Item
            {
                Name = itemDto.Name,
                ExpiryDate = itemDto.ExpiryDate
            };
        }

    }
}