using SimpleAPI.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.Core.Entities;
public sealed class Department:BaseEntity
{
    public string? DepartmentName { get; set; }
    public ICollection<Staff>? Staffs { get; set; }

}
