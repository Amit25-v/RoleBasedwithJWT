using Dapper;
using DataAcessLayer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public RoleService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void AddRole(RoleviewModels role)
        {
            try
            {
                DynamicParameters parm = new DynamicParameters();
                parm.Add("@RoleName", role.RoleName);
                _connection.Execute("CreateRole", parm, commandType: CommandType.StoredProcedure);


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

        public List<RoleviewModels> ListRole()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                    var result = _connection.Query<RoleviewModels>(
                        "ListRoles",
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    }
