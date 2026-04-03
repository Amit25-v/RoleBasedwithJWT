using BusinessLayer.EmployeeService;
using DataAcessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;
using System.Collections;
using SieveModel = Sieve.Models.SieveModel;

namespace Web_Api_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeSerivce _empService;
        private readonly SieveProcessor _seiev;
        public EmployeeController(IEmployeeSerivce empService, SieveProcessor seiev)
        {
            _seiev = seiev;
            _empService = empService;
        }

       [Authorize(Roles = "Admin,HR")]
       [HttpPost("EmpAdd")]

        public IActionResult EmpAdd([FromBody] EmployeeView emp)
        {
            if(!ModelState.IsValid)
    {
                // Automatically collects all errors
                return BadRequest(ModelState);
            }
            _empService.AddEmp(emp);

            return Ok("Employee added successfully.");
        }
        //[Authorize(Roles = "Admin,HR")]

        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            var result = _empService.GetListEmp(pageNumber, pageSize, name);

            return Ok(new
            {
                TotalCount = result.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = result.Employees 
            });
        }
        [HttpGet("GetByID")]

        public IActionResult GetByID(int EmployeeId)

        {
            var result = _empService.GetById(EmployeeId);

            return Ok(result);
        }   
        [HttpPut("Update")]

        public IActionResult Update(EmployeeTable emp)
        {
            if (!ModelState.IsValid)
            {
                // Automatically collects all errors
                return BadRequest(ModelState);
            }
            _empService.UPdate(emp);
                            return Ok(new { message = "Employee Updatesuccessfully" });
        }
        [HttpDelete("Delete")]

        public IActionResult Delete(int EmployeeId)
        {
            _empService.Delete(EmployeeId);
            return Ok(new { message = "Employee Delete successfully" });

        }

        //[HttpGet("search")]

        //[HttpGet]
        //[HttpGet("filter")]
        //public async Task<IActionResult> FilterEmployee([FromQuery] EmployeeView filter)
        //{
        //    var result = await _empService.AddListAsync(
        //        filter.Name,
        //        filter.Addrees,
        //        filter.Email_Id,
        //        filter.City);

        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Employees fetched successfully",
        //        data = result
        //    });
        //}
    }
}