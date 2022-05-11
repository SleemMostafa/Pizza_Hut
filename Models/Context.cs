using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Hut.Models
{
    public class Context:IdentityDbContext<Customer>
    {
        public Context() : base()//onconfigu
        {

        }
        public Context(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Cart> carts { set; get; }
        public DbSet<Category> categories { set; get; }
        public DbSet<Customer> customers { set; get; }
        public DbSet<Order> orders { set; get; }
        public DbSet<Product> products { set; get; }
        public DbSet<ProductSize> productSizes { set; get; }
        public DbSet<ProductSizeOrder> productSizeOrders { set; get; }
        public DbSet<ProductSizeCart>  ProductSizeCarts { set; get; }
        public DbSet<Comment> Comments { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=PizzaHut;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Name Tables Without S
            modelBuilder.Entity<Category>().ToTable("Cateory");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductSizeCart>().ToTable("ProductSizeCart");
            modelBuilder.Entity<ProductSize>().ToTable("ProductSize");
            modelBuilder.Entity<ProductSizeOrder>().ToTable("ProductSizeOrder");


            //Start Customer
        
            modelBuilder.Entity<Customer>().HasMany(c => c.Orders).WithOne(o => o.Customer);
            //End Customer

            //Start Category
            modelBuilder.Entity<Category>().HasKey(c => c.ID);
            //End Categor

            //Cart
            modelBuilder.Entity<Cart>().HasOne(c => c.Customer).WithOne(c => c.Cart);
            //

        }
    }
}