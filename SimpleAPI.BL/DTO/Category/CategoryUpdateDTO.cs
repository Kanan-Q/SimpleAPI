using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.BL.DTO.Category;
public sealed record CategoryUpdateDTO
{
    public string CategoryName { get; init; }
}
