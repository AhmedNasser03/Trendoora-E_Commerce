using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using E_Commerce.Data.Consts;
using E_Commerce.Data.Models;

namespace E_Commerce.Data.Data
{
	public class E_CommerceContext : IdentityDbContext<ApplicationUser>
	{
		public E_CommerceContext(DbContextOptions<E_CommerceContext> options) : base(options)
		{
		}
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<Brand> Brand { get; set; }
		public DbSet<Cart> Cart { get; set; }
		public DbSet<CartItems> CartItems { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Order> Order { get; set; }
		public DbSet<OrderItems> OrderItems { get; set; }
		public DbSet<Payment> Payment { get; set; }
		public DbSet<Product> Product { get; set; }
		public DbSet<Reviews> Review { get; set; }
		public DbSet<DesireList> DesireList { get; set; }
		public DbSet<DesireListItems> DesireListItem { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Composite primary keys
			modelBuilder.Entity<OrderItems>().HasKey(oi => new { oi.ProductId, oi.OrderId });
			modelBuilder.Entity<CartItems>().HasKey(ci => new { ci.CartId, ci.ProductId });
			modelBuilder.Entity<DesireListItems>().HasKey(dl => new { dl.DesireListId, dl.ProductId });

			// Seed roles
			SeedRoles(modelBuilder);
		}

		private void SeedRoles(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole { Name = UserType.Buyer, NormalizedName = UserType.Buyer.ToUpper() },
				new IdentityRole { Name = UserType.Seller, NormalizedName = UserType.Seller.ToUpper() },
				new IdentityRole { Name = UserType.Admin, NormalizedName = UserType.Admin.ToUpper() }
			);
		}
	}
}
