using BusinessLayer.Interfaces;
using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        // dependancy injection
        private readonly CompanyDbContext _Context;

        // to work dependancy injection should be in class Startup on ConfigureServices [services.AddDbContext<CompanyDbContext>();]
        public EmployeeRepository(CompanyDbContext dbContext) : base(dbContext) // ask clr for object from dbcontext 
        {
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _Context.Employees.Where(e => e.Address.Contains(address));
        }
    }
}
