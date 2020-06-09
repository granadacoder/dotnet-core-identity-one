namespace GranadaCoder.IdentityDemo.Bal.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GranadaCoder.IdentityDemo.Bal.Managers.Interfaces;
    using GranadaCoder.IdentityDemo.Dal.Interfaces;
    using GranadaCoder.IdentityDemo.Domain;
    using Microsoft.Extensions.Logging;

    public class EmployeeManager : IEmployeeManager
    {
        private readonly ILogger<EmployeeManager> logger;
        private readonly IEmployeeDataLayer iedl;

        public EmployeeManager(ILoggerFactory loggerFactory, IEmployeeDataLayer iedl)
        {
            this.logger = loggerFactory.CreateLogger<EmployeeManager>();
            this.iedl = iedl;
        }

        public void DoSomething()
        {
            this.logger.LogInformation("DoSomething Started.");
            int numer = 6;
            int denom = 2;
            int x = numer / denom;
            this.logger.LogInformation(string.Format("{0} div {1} = {2}", numer, denom, x));
        }

        public async Task<ICollection<Employee>> GetByDateOfBirth(DateTime dateOfBirth)
        {
            ICollection<Employee> returnItem = await this.iedl.GetByDateOfBirth(dateOfBirth);
            return returnItem;
        }

        public async Task<Employee> GetByID(int id)
        {
            Employee returnItem = await this.iedl.GetByID(id);
            return returnItem;
        }
    }
}