using Microsoft.EntityFrameworkCore;
#nullable disable
namespace Packt.Shared
{
  public partial class ParkingLot : DbContext
  {
    public ParkingLot()
    {
    }
    public ParkingLot(DbContextOptions<ParkingLot> options)
      : base(options)
    {
    }
    public virtual DbSet<ParkingSpot> ParkingSpots { get; set; } 

    protected override void OnConfiguring(
      DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlite("Filename=../Northwind.db");
      }
    }
    protected override void OnModelCreating(
      ModelBuilder modelBuilder)
    { 
      modelBuilder.Entity<ParkingSpot>()
        .Property(spot => spot.Rate)
        .HasConversion<double>();
      OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}