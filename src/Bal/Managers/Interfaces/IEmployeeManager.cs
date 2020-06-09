namespace GranadaCoder.IdentityDemo.Bal.Managers.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GranadaCoder.IdentityDemo.Domain;

    public interface IEmployeeManager
    {
        void DoSomething();

        Task<Employee> GetByID(int id);

        Task<ICollection<Employee>> GetByDateOfBirth(DateTime dateOfBirth);
    }
}
