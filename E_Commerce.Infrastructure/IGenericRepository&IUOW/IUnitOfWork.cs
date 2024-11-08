using E_Commerce.Data.Models;

namespace E_Commerce.Infrastructure.IGenericRepository_IUOW
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> User { get; set; }
        IGenericRepository<Brand> Brand { get; set; }
        IGenericRepository<Cart> Cart { get; set; }
        IGenericRepository<CartItems> CartItems { get; set; }
        IGenericRepository<Category> Category { get; set; }
        IGenericRepository<DesireList> DesireList { get; set; }
        IGenericRepository<DesireListItems> DesireListItems { get; set; }
        IGenericRepository<Order> Order { get; set; }
        IGenericRepository<OrderItems> OrderItems { get; set; }
        IGenericRepository<Payment> Payment { get; set; }
        IGenericRepository<Product> Product { get; set; }
        IGenericRepository<Reviews> Reviews { get; set; }


        Task CreateTransactionAsync();
        Task CommitAsync();
        Task CreateSavePointAsync(string point);
        Task RollbackAsync();
        Task RollbackToSavePointAsync(string point);
        Task<int> SaveAsync();
    }
}
