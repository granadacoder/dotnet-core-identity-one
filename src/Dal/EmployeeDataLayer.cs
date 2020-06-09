using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GranadaCoder.IdentityDemo.Dal.Interfaces;
using GranadaCoder.IdentityDemo.Domain;
using Microsoft.Extensions.Configuration;

namespace GranadaCoder.IdentityDemo.Dal
{
    public class EmployeeDataLayer : IEmployeeDataLayer
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;

        public EmployeeDataLayer(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(this.config.GetConnectionString("MyConnectionString"));
            }
        }

        public async Task<Employee> GetByID(int id)
        {
            using (IDbConnection conn = this.Connection)
            {
                string sql = "SELECT ID, FirstName, LastName, DateOfBirth FROM Employee WHERE ID = @ID";
                sql = "SELECT TOP 1 id as ID, 'FName' + name as FirstName, 'LName' + name as LastName, crdate as DateOfBirth FROM sysobjects order by id";
                conn.Open();
                var result = await conn.QueryAsync<Employee>(sql, new { ID = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<ICollection<Employee>> GetByDateOfBirth(DateTime dateOfBirth)
        {
            using (IDbConnection conn = this.Connection)
            {
                string sql = "SELECT ID, FirstName, LastName, DateOfBirth FROM Employee WHERE DateOfBirth = @DateOfBirth";
                sql = "SELECT TOP 3 id as ID, 'FName' + name as FirstName, 'LName' + name as LastName, crdate as DateOfBirth FROM sysobjects order by id";

                conn.Open();
                var result = await conn.QueryAsync<Employee>(sql, new { DateOfBirth = dateOfBirth });
                return result.ToList();
            }
        }
    }
}
