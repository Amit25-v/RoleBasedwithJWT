using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.RegisterService
{
    public interface IRegisetrService
    {
        public void Register(RoleViewModel model);

        public Login Login(LoginViewModel model);   


    }
}
