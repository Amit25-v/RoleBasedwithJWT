using BusinessLayer.RoleService;
using Dapper;
using DataAcessLayer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Web.Mvc.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.RegisterService
{
    public class RegisetrServices : IRegisetrService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly IRoleService _role;

        public RegisetrServices(IConfiguration configuration, IRoleService role)
        {
            _role = role;
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void Register(RoleViewModel model)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                // Hash the password

                var parm = new DynamicParameters();
                parm.Add("@Name", model.Name ?? "");
                parm.Add("@Address", model.Address ?? "");
                parm.Add("@Email", model.Email);
                parm.Add("@Password", EncryptPaaword(model.Password));
                parm.Add("@RoleId", model.RoleId);  

                _connection.Execute("sp_AddRegister", parm, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Registration failed: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
        private string GenerateToken(string email, string RegisterId, List<string> roles)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new List<Claim>
    {
       new Claim(ClaimTypes.NameIdentifier, RegisterId.ToString()),  // This is the UserId
        new Claim(ClaimTypes.Email, email)

    };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public Login? Login(LoginViewModel model)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                // Get user by email
                var users = _connection.Query<RegisterTable>(
                    "sp_Login",
                    new { Email = model.Email,Password = DecryptPassword(model.Password) },
                    commandType: CommandType.StoredProcedure
                ).ToList();

                var user = users.FirstOrDefault();

                // Check if user exists and password matches
               

                // Get distinct roles for the user
                var roles = users
                    .Where(u => !string.IsNullOrEmpty(u.RoleName))
                    .Select(u => u.RoleName!)
                    .Distinct()
                    .ToList();

                // Generate JWT token
                var token = GenerateToken(user.Email!, user.RegisterId.ToString(), roles);

                return new Login
                {
                    Token = token,
                    Email = user.Email,
                    Name = user.Name,
                    Roles = roles,
                    Address = user.Address
                };
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
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
        public static string? DecryptPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            try
            {
                byte[] bytes = Convert.FromBase64String(password.Trim());
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                // Invalid Base64 string
                return null;
            }
            catch (Exception)
            {
                // Optional: log error here
                return null;
            }
        }
    }
}