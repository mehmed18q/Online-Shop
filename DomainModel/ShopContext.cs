using Microsoft.EntityFrameworkCore;
namespace DomainModel
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductFeature> ProductFeatures { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserType>().HasData(
                new UserType() { Id = 1, Name = "User", Title = "کاربر" },
                new UserType() { Id = 2, Name = "Admin", Title = "مدیر" });

            modelBuilder.Entity<User>().HasData(
                new User() { 
                    Id = 1,
                    UserTypeId = 2,
                    FullName = "مدیر سیستم",
                    Email = "admin@shop.com", 
                    Password = "admin@shop" ,
                    Address = "Qom",
                    MobileNumber = "+989217074647",
                    CreatedAt = DateTime.Now,
                    CreatedBy = 1,        
                });

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus() { Id = 1, Name = "Pending payment", Title = "در انتظار پرداخت" },
                new OrderStatus() { Id = 2, Name = "Paid", Title = "پرداخت شده" },
                new OrderStatus() { Id = 3, Name = "Error occurred", Title = "خطای پرداخت" },
                new OrderStatus() { Id = 4, Name = "Canceled", Title = "لغو شده" },
                new OrderStatus() { Id = 5, Name = "Returned", Title = "مرجوع شده" }
                );
        }
    }
}