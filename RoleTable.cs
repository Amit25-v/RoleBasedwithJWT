using System;
using System.Collections.Generic;
using System.Text;

namespace DataAcessLayer
{
        public class RoleTable
        {
            public int RoleId { get; set; }
            public string? RoleName { get; set; }
        public ICollection<EmployeeTable>? Employees { get; set; }

    }
}
    