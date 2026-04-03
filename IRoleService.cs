using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.RoleService
{
    public interface IRoleService
    {
        public void AddRole(RoleviewModels role);
        public List<RoleviewModels> ListRole();
    }
}
