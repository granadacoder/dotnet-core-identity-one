namespace GranadaCoder.IdentityDemo.Dal.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GranadaCoder.IdentityDemo.Domain;

    public interface IEmployeeDataLayer
    {
        Task<Employee> GetByID(int id);

        Task<ICollection<Employee>> GetByDateOfBirth(DateTime dateOfBirth);
    }
}
