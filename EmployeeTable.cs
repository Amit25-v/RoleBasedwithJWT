using Microsoft.AspNetCore.SignalR.Protocol;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAcessLayer
{
    public class EmployeeTable
    {
        public int EmployeeId { get; set; }

        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Addrees { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email_Id { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6)]
        public string? Password { get; set; }

        public int RoleId { get; set; }

        // ✅ Navigation Property
        public RoleTable? Role { get; set; }
    }
}
