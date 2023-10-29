using BusinessLayer.Interfaces;
using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // dependancy injection
        private readonly CompanyDbContext _Context;

        // to work dependancy injection should be in class Startup on ConfigureServices [services.AddDbContext<CompanyDbContext>();]
        public GenericRepository(CompanyDbContext dbContext) // ask clr for object from dbcontext 
        {
            this._Context = dbContext;
        }

        public async Task Add(T item)
        {
            await _Context.AddAsync(item);
        }

        public void Update(T item)
        {
            _Context.Update(item);
        }

        public void Delete(T item)
        {
            _Context.Remove(item);
        }

        public async Task< IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _Context.Employees.Include(e => e.Department).ToListAsync();

            return await _Context.Set<T>().ToListAsync();
        }

        public async Task< T> GetById(int id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }
    }
}
