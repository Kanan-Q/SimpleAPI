using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.BL.DTO.Department
{
    public sealed record DepartmentUpdateDTO
    {
        public string DepartmentName { get; init; }
    }
}
