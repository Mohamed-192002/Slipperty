using Castle.Core.Resource;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Infrastructure.Migrations.Models;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //  public ApplicationDbContext()
        // {
        // }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Manufacturing> Manufacturing { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<ProductVideo> ProductVideos { get; set; }
        //public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<ProductCountsOffer> ProductCountsOffers { get; set; }
        public DbSet<VariableCombination> VariableCombinations { get; set; }
        public DbSet<VariableValue> VariableValues { get; set; }
        public DbSet<ProductVariable> ProductVariables { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<BlockedNumber> BlockedNumbers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<LinkType> LinkTypes { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Government> Governments { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHead> OrderHeads { get; set; }
        public DbSet<PixelSetting> PixelSettings { get; set; }
        public DbSet<ContactStatus> ContactStatuses { get; set; }
        public DbSet<ContactMethod> ContactMethods  { get; set; }
        public DbSet<OrderTransaction> OrderTransactions { get; set; }
        public DbSet<OrderFollowUps> OrderFollowUps { get; set; }
        public DbSet<OrderCancelationDetails> OrderCancelationDetails { get; set; }
        public DbSet<OrderUrgentDetails> OrderUrgentDetails { get; set; }
        public DbSet<OrderHoldingDetails> OrderHoldingDetails { get; set; }
        public DbSet<OrderModificationDeclined> OrderModificationDeclined { get; set; }
        // public DbSet<OperationType> OperationTypes { get; set; }
        // public DbSet<OrderStatus> OrderStatuses { get; set; }

        //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Data Source=DESKTOP-V2RG6QB\\SQLEXPRESS;Database=db10676;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"); 
        //     base.OnConfiguring(optionsBuilder);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.MainImage)
            //    .WithMany()
            //    .HasForeignKey(p => p.MainImageId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.IconImage)
            //    .WithMany()
            //    .HasForeignKey(p => p.IconImageId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Product>()
            //   .HasOne(p => p.Category)
            //   .WithMany()
            //   .HasForeignKey(p => p.CategoryId)
            //   .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.SubCategory)
            //    .WithMany()
            //    .HasForeignKey(p => p.SubCategoryId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.MainImage)
            //    .WithMany()
            //    .HasForeignKey(p => p.MainImageId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.IconImage)
            //    .WithMany()
            //    .HasForeignKey(p => p.IconImageId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for ProductColor
            //modelBuilder.Entity<ProductColor>()
            //    .HasOne(pc => pc.Product)
            //    .WithMany(p => p.ProductColors)
            //    .HasForeignKey(pc => pc.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for ProductSize
            //modelBuilder.Entity<ProductSize>()
            //    .HasOne(ps => ps.Product)
            //    .WithMany(p => p.ProductSizes)
            //    .HasForeignKey(ps => ps.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for ProductCountsOffer
            modelBuilder.Entity<ProductCountsOffer>()
                .HasOne(pco => pco.Product)
                .WithMany(p => p.ProductCountsOffers)
                .HasForeignKey(pco => pco.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for RelatedProduct
            //modelBuilder.Entity<RelatedProduct>()
            //    .HasOne(rp => rp.Product)
            //    .WithMany(p => p.RelatedProducts)
            //    .HasForeignKey(rp => rp.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction);


            // Configure relationships for ProductVideo
            modelBuilder.Entity<ProductVideo>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVideos)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationships for ProductVariable
            modelBuilder.Entity<ProductVariable>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVariables)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductCategory>()
       .HasKey(pc => new { pc.ProductId, pc.CategoryId });  // Composite key

            // Configure the relationship between Product and ProductCategory
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.Categories)  // Reverse navigation on the Product model
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.NoAction);  // Delete behavior configuration


            // Configure composite key for ProductSubCategory
            modelBuilder.Entity<ProductSubCategory>()
                .HasKey(psc => new { psc.ProductId, psc.SubCategoryId });  // Composite key

            // Configure the relationship between Product and ProductSubCategory
            modelBuilder.Entity<ProductSubCategory>()
                .HasOne(psc => psc.Product)        // A ProductSubCategory has one Product
                .WithMany(p => p.SubCategories)    // A Product has many ProductSubCategories
                .HasForeignKey(psc => psc.ProductId) // ForeignKey in ProductSubCategory table
                .OnDelete(DeleteBehavior.NoAction); // Delete behavior configuration


            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Material)
                .WithMany()
                .HasForeignKey(p => p.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserAddress>()
                .HasOne(u => u.Region)
                .WithMany()
                .HasForeignKey(u => u.RegionId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(u => u.Combination)
                .WithMany()
                .HasForeignKey(u => u.CombinationId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(u => u.ProductCountsOffer)
                .WithMany()
                .HasForeignKey(u => u.offerId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<OrderDetails>()
                .HasOne(u => u.Combination)
                .WithMany()
                .HasForeignKey(u => u.CombinationId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OrderDetails>()
                .HasOne(u => u.ProductCountsOffer)
                .WithMany()
                .HasForeignKey(u => u.offerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Disable cascading delete for the relationships
            modelBuilder.Entity<OrderHead>()
                .HasOne(o => o.Government)
                .WithMany()
                .HasForeignKey(o => o.governmentId)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            modelBuilder.Entity<OrderHead>()
                .HasOne(o => o.Region)
                .WithMany()
                .HasForeignKey(o => o.regionId)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            modelBuilder.Entity<OrderHead>()
                .HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(o => o.paymentMethodId)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete


            modelBuilder.Entity<OrderDetails>(entry =>
            {
                entry.ToTable("OrderDetails", tb => tb.HasTrigger("trg_UpdateStockCount"));
            });

        }


    }

    //public override int SaveChanges()
    //{
    //    var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    //    foreach (var entry in ChangeTracker.Entries())
    //    {
    //        if (entry.Entity is TrackEntity trackEntity)
    //        {
    //            if (entry.State == EntityState.Added)
    //            {
    //                // Set RegDate and RegUserId for new entries
    //                trackEntity.RegDate = DateTime.UtcNow;
    //                trackEntity.RegUserId = currentUserId;
    //            }
    //            else if (entry.State == EntityState.Modified)
    //            {
    //                // Set EditDate and EditUserId for modified entries
    //                trackEntity.EditDate = DateTime.UtcNow;
    //                trackEntity.EditUserId = currentUserId;
    //            }
    //        }
    //    }

    //    return base.SaveChanges();
    //}

}

