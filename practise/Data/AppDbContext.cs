using Microsoft.EntityFrameworkCore;
using practise.Models;
using System;

namespace practise.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet <Order> Orders { get; set; }
        public DbSet <OrderItem> OrderItems { get; set; }

        public DbSet <Category> Categories { get; set; }
        
            
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            const string adminPasswordHash = "$2a$11$HAtPtxCggkice9gGc8Q/3uiKZ1p0ljgqmX.JcTdZ.wDnGUvqJTiei";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Admin",
                    Email = "admins@gmail.com",
                    PasswordHash = adminPasswordHash,
                    Role = "Admin"
                });

            // User and Cart Relation
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.userId);

            // Cart & CartItem Relation
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.cartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(c => c.cartId);

            // CartItems and Product Relation
            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItem)
                .HasForeignKey(ci => ci.ProductId);

            // Address and User Relation
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.Userid);


            // Order and User Relation
            // Use: Sets up a one-to-many relationship between User and Order. One User can have many Orders, and each Order belongs to one User. The foreign key is UserId in the Order table. If a User is deleted, the related Orders are not automatically affected (NoAction), so you must handle them manually to avoid cascade issues.
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.userId)
                .OnDelete(DeleteBehavior.NoAction);

            // Order and Address Relation
            // Use: Sets up a many-to-one relationship between Order and Address. Many Orders can be linked to one Address (e.g., shipping address), and each Order has one Address. The foreign key is AddressId in the Order table. If an Address is deleted, all related Orders are automatically deleted (Cascade).
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique Index on TransactionId
            // Use: Creates a unique index on the TransactionId property of the Order entity, ensuring no two Orders can have the same TransactionId. This prevents duplicate transactions (e.g., from a payment gateway) and enforces uniqueness at the database level.
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.TransactionId)
                .IsUnique();

            // Default Value for OrderStatus
            // Use: Sets a default value of "Pending" for the OrderStatus property of the Order entity. When a new Order is created without specifying an OrderStatus, it will automatically be set to "Pending" in the database.
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasDefaultValue("Pending");


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Product)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}