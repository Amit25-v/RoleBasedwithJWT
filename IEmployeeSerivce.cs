using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.EmployeeService
{
    public interface IEmployeeSerivce
    {
        public void AddEmp(EmployeeView emp);
        public PagedEmployeeResult GetListEmp(int pageNumber, int pageSize, string Name);
        public EmployeeView GetById(int EmlployeeId);
          public void UPdate(EmployeeTable emp);
        public int Delete(int EmlployeeId);
        //Task<IEnumerable<EmployeeView>> AddListAsync(string Name, string Addrees, string Email_Id, string  City);
    }
}
