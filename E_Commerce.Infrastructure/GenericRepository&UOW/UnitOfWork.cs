using Microsoft.EntityFrameworkCore.Storage;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using E_Commerce.Data.Data;

namespace E_Commerce.Infrastructure.GenericRepository_UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction transaction;

        private readonly E_CommerceContext _context;
		public virtual IGenericRepository<ApplicationUser> User { get; set; }
		public virtual IGenericRepository<Brand> Brand { get; set; }
		public virtual IGenericRepository<Cart> Cart { get; set; }
		public virtual IGenericRepository<CartItems> CartItems { get; set; }
		public virtual IGenericRepository<Category> Category { get; set; }
		public virtual IGenericRepository<DesireList> DesireList { get; set; }
		public virtual IGenericRepository<DesireListItems> DesireListItems { get; set; }
		public virtual IGenericRepository<Order> Order { get; set; }
		public virtual IGenericRepository<OrderItems> OrderItems { get; set; }
		public virtual IGenericRepository<Payment> Payment { get; set; }
		public virtual IGenericRepository<Product> Product { get; set; }
		public virtual IGenericRepository<Reviews> Reviews { get; set; }
		public UnitOfWork(E_CommerceContext context)
        {
            _context = context;
            User = new GenericRepository<ApplicationUser>(_context);
			Brand = new GenericRepository<Brand>(_context);
			Cart = new GenericRepository<Cart>(_context);
			CartItems = new GenericRepository<CartItems>(_context);
			Category = new GenericRepository<Category>(_context);
			DesireList = new GenericRepository<DesireList>(_context);
			DesireListItems = new GenericRepository<DesireListItems>(_context);
			Order = new GenericRepository<Order>(_context);
			OrderItems = new GenericRepository<OrderItems>(_context);
			Payment = new GenericRepository<Payment>(_context);
			Product = new GenericRepository<Product>(_context);
			Reviews = new GenericRepository<Reviews>(_context);
        }

        public async Task CreateTransactionAsync()
        {
            transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await transaction.CommitAsync();
        }

        public async Task CreateSavePointAsync(string point)
        {
            await transaction.CreateSavepointAsync(point);
        }

        public async Task RollbackAsync()
        {
            await transaction.RollbackAsync();
        }

        public async Task RollbackToSavePointAsync(string point)
        {
            await transaction.RollbackToSavepointAsync(point);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
