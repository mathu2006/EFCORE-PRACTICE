using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREPractice.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
