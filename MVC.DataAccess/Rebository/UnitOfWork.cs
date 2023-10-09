

using MVC.DataAccess.IRebository;
using MVC.Model;

namespace MVC.DataAccess.Rebository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        private IGenericRepository<Category> _category;
        private IGenericRepository<Product> _product;
        public IGenericRepository<Category> Category => _category  ??=new GenericRepository<Category>(_context);
        public IGenericRepository<Product> Product => _product ??= new GenericRepository<Product>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
