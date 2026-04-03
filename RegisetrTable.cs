using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAcessLayer
{
    public class RegisterTable
    {
        public int RegisterId { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email{ get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }
        public string? RoleName { get; set; }
        public IFormFile ? File { get; set; }

    }
}
