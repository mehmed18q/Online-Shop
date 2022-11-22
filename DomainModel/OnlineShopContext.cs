using Microsoft.EntityFrameworkCore;
namespace DomainModel
{
    public class OnlineShopContext:DbContext
    {
        public OnlineShopContext(DbContextOptions options):base(options)
        { }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductFeatures> ProductFeatures { get; set; }
        public virtual DbSet<ProductImages> GetProductImages { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {
            modelBuilder.Entity<UserType>().HasData(
                new UserType() { Id = 1, Name = "User", Title = "کاربر" },
                new UserType() { Id = 2, Name = "Admin", Title = "مدیر" });

            modelBuilder.Entity<User>().HasData(
                new User() {Id=1, UserTypeId=2, FullName="مدیر سیستم", Email="admin", Password="123" });

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
