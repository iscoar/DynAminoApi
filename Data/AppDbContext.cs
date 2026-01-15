using Microsoft.EntityFrameworkCore;
using DynAmino.Models;

namespace DynAmino.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Proceso> Procesos { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseOrderDetail>()
            .HasKey(pod => new { pod.PurchaseOrderNo, pod.PurchaseOrderLineNo });

        modelBuilder.Entity<PurchaseOrder>()
            .HasMany(po => po.PurchaseOrderDetails)
            .WithOne(pod => pod.PurchaseOrder)
            .HasForeignKey(pod => pod.PurchaseOrderNo);
    }
}
