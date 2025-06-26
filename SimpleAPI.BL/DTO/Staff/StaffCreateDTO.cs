using SimpleAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.BL.DTO.Staff
{
    public sealed record StaffCreateDTO
    {
        public string Name { get; init; }
        public string Surmame { get; init; }
        public DateOnly DateOfBirth { get; init; }
        public decimal Salary { get; init; }
        public int DepartmentId { get; init; }
    }
}

    
    

