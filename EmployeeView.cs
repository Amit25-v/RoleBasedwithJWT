using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAcessLayer
{
    public class EmployeeView
    {
        public int EmployeeId { get; set; }

        public string? Name { get; set; }
        public string? Addrees { get; set; }
        public string? Email_Id { get; set; }
        public string? City { get; set; }
        public string ? Password { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
