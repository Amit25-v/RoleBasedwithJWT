using Dapper;
using DataAcessLayer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.Mvc.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace BusinessLayer.EmployeeService
{
    public class EmployeeSerivces : IEmployeeSerivce
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public EmployeeSerivces(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void AddEmp(EmployeeView emp)
        {
            try
            {

                DynamicParameters parm = new DynamicParameters();
                parm.Add("@Name", emp.Name);
                parm.Add("@Addrees ", emp.Addrees);
                parm.Add("@Email_Id", emp.Email_Id);
                parm.Add("@City", emp.City);
                parm.Add("@Password", EncryptPaaword(emp.Password));
                parm.Add("@RoleId", emp.RoleId);

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                var Employeeid = _connection.Execute("EmpAdd", parm, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }


        //public async Task<IEnumerable<EmployeeView>> AddListAsync(string Name, string Addrees, string Email_Id, string City)
        //{
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@Name", string.IsNullOrWhiteSpace(Name) ? null : Name);
        //    parameters.Add("@Addrees", string.IsNullOrWhiteSpace(Addrees) ? null : Addrees);
        //    parameters.Add("@Email_Id", string.IsNullOrWhiteSpace(Email_Id) ? null : Email_Id);
        //    parameters.Add("@City", string.IsNullOrWhiteSpace(City) ? null : City);

        //    var result = await _connection.QueryAsync<EmployeeView>(
        //        "FilterList",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    return result;
        //}
        public int Delete(int EmployeeId)
        {
            try
            {

                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }

                DynamicParameters parm = new DynamicParameters();
                parm.Add("@EmployeeId", EmployeeId);

                var result = _connection.Execute(
                                "EmpDelete",
                                parm,
                                commandType: CommandType.StoredProcedure);

                return result;
            }
            finally
            {
                _connection.Close();
            }
        }

        public EmployeeView GetById(int EmployeeId)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                DynamicParameters parm = new DynamicParameters();
                parm.Add("@EmployeeId", EmployeeId);
                var result = _connection.QueryFirstOrDefault<EmployeeView>(
                          "EmpGetById",
                          parm,
                          commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public PagedEmployeeResult GetListEmp(int pageNumber, int pageSize, string? name)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@Name", name);

                using (var multi = _connection.QueryMultiple(
                    "GetListEmp",
                    parameters,
                    commandType: CommandType.StoredProcedure))
                {
                    var totalCount = multi.ReadFirst<int>();
                    var employees = multi.Read<EmployeeView>().ToList(); // ✅ changed

                    return new PagedEmployeeResult
                    {
                        TotalCount = totalCount,
                        Employees = employees
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching employees", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        
        public void UPdate(EmployeeTable emp)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(emp.Password);

                DynamicParameters parm = new DynamicParameters();
                parm.Add("@EmployeeId", emp.EmployeeId);
                parm.Add("@Name", emp.Name);
                parm.Add("@Email_Id", emp.Email_Id);
                parm.Add("@Addrees", emp.Addrees); // ✅ MISSING PARAMETER FIXED
                parm.Add("@City", emp.City);
                parm.Add("@Password", hashedPassword);
                parm.Add("@RoleId", emp.RoleId);

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                _connection.Execute("EmpUpdate", parm, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }
        public static string EncryptPaaword(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return null;
            }
            else
            {
                byte[] storepassword = ASCIIEncoding.ASCII.GetBytes(Password);
                string EncryptPaaword = Convert.ToBase64String(storepassword);
                return EncryptPaaword;
            }
        }
        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return null;

            try
            {
                byte[] bytes = Convert.FromBase64String(password);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                // Handle invalid Base64 safely
                return null; // Or throw a custom error
            }
        }
    }
}

