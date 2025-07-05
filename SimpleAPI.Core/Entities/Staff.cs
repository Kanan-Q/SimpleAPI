using SimpleAPI.Core.Entities.Common;

namespace SimpleAPI.Core.Entities
{
    public sealed class Staff:BaseEntity
    {
        public string Name { get; set; }
        public string Surmame { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

    }
}
