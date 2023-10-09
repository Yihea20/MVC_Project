
using MVC.Model;
using System;

namespace MVC.DataAccess.IRebository
{
    public interface IUnitOfWork : IDisposable
    {
      public IGenericRepository<Category> Category { get; }
        public IGenericRepository<Product>Product { get; } 
        Task Save();


    }
}
