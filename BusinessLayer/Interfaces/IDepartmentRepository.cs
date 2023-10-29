using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    // interface included
    // 1) signature of property
    // 2) signature of method
    // 3) default implement of method
    public interface IDepartmentRepository : IGenericRepository<Department>
    {

    }
}
