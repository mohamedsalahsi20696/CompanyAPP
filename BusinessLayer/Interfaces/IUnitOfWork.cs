using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; set; }
        IDepartmentRepository DepartmentRepository { get; set; }
        public Task<int> Complete();
    }
}
