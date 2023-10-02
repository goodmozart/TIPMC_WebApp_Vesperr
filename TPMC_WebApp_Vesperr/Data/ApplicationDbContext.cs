using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection.Emit;
using TIPMC_WebApp_Vesperr.Models;
using TIPMC_WebApp_Vesperr.Models.Online;
using TIPMC_WebApp_Vesperr.Models.POS;
using TIPMC_WebApp_Vesperr.Models.TIPMC;

namespace TIPMC_WebApp_Vesperr.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        internal readonly IQueryable<OrderItem> OrderItems;

        public DbSet<UserAudit> UserAuditEvents { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //Product
            builder.Entity<Product>()
           .Property(p => p.PricePurchase)
           .HasColumnType("decimal(18,4)");
            builder.Entity<Product>()
           .Property(p => p.PriceSell)
           .HasColumnType("decimal(18,4)");
            //PurchaseOrderLine
            builder.Entity<PurchaseOrderLine>()
            .Property(p => p.Discount)
            .HasColumnType("decimal(18,4)");
            builder.Entity<PurchaseOrderLine>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,4)");
            builder.Entity<PurchaseOrderLine>()
        .Property(p => p.SubTotal)
        .HasColumnType("decimal(18,4)");
            builder.Entity<PurchaseOrderLine>()
        .Property(p => p.Total)
        .HasColumnType("decimal(18,4)");
            //SalesOrderLine
            builder.Entity<SalesOrderLine>()
         .Property(p => p.Discount)
         .HasColumnType("decimal(18,4)");
            builder.Entity<SalesOrderLine>()
          .Property(p => p.Price)
          .HasColumnType("decimal(18,4)");
            builder.Entity<SalesOrderLine>()
         .Property(p => p.SubTotal)
         .HasColumnType("decimal(18,4)");
            builder.Entity<SalesOrderLine>()
        .Property(p => p.Total)
        .HasColumnType("decimal(18,4)");
            //Online
            builder.Entity<CartItem>(entity =>
            {
                entity.HasKey(p => p.ProductId);
            });
            builder.Entity<CartViewModel>()
        .Property(p => p.GrandTotal)
        .HasColumnType("decimal(18,4)");
            builder.Entity<CartViewModel>().HasNoKey();
            builder.Entity<CartItem>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,4)");
            //MemberShare
            builder.Entity<MemberShares>()
        .Property(p => p.Amount)
        .HasColumnType("decimal(18,4)");
            //OrderItem
            builder.Entity<OrderItem>()
         .Property(p => p.Price)
         .HasColumnType("decimal(18,4)");
            builder.Entity<OrderItem>()
        .Property(p => p.Total)
        .HasColumnType("decimal(18,4)");
            //Order
            builder.Entity<Order>()
         .Property(p => p.TotalAmount)
         .HasColumnType("decimal(18,4)");
            //MemberPayment
            builder.Entity<MemberPayment>()
        .Property(p => p.Principal)
        .HasColumnType("decimal(18,4)");
            builder.Entity<MemberPayment>()
       .Property(p => p.Interest)
       .HasColumnType("decimal(18,4)");
        }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.Member> Member { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.Vendor> Vendor { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.Product> Product { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.SalesOrder> SalesOrder { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.GoodsReceive> GoodsReceive { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.InvenTran> InvenTran { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.PurchaseOrderLine> PurchaseOrderLine { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.SalesOrderLine> SalesOrderLine { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.POS.GoodsReceiveLine> GoodsReceiveLine { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.Online.ProductOnline> Products { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.Online.Category> Categories { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.Online.CartItem> Cart { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.Online.Order> Orders { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.Chat.Message> Messages { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.TIPMC.MemberShares> MemberShares { get; set; }
        public DbSet<TIPMC_WebApp_Vesperr.Models.TIPMC.MemberPayment> MemberPayment { get; set; }

    }
}
