using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.RegisterService
{
    public class Login
    {
        public string Token { get; set; } = null!;
        public string ? Email { get; set; }   
        public string ? Name { get; set; }   
        public string ? Address { get; set; }   
        public List<string> Roles { get; set; }    
    }
}
