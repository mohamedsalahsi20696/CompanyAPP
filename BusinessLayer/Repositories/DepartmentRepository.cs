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
    // class DepartmentRepository implement interface IDepartmentRepository
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        // dependancy injection
        private readonly CompanyDbContext _Context;

        // to work dependancy injection should be in class Startup on ConfigureServices [services.AddDbContext<CompanyDbContext>();]
        public DepartmentRepository(CompanyDbContext dbContext) : base(dbContext) // ask clr for object from dbcontext 
        {
        }
    }
}
