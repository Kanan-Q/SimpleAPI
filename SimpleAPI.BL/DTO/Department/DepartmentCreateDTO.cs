using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.BL.DTO.Department;
public sealed record DepartmentCreateDTO
{
    public string DepartmentName { get; init; }
}
