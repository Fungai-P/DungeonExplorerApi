using DungeonExplorerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DungeonExplorerApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Map> Maps => Set<Map>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var map = modelBuilder.Entity<Map>();

            map.HasKey(m => m.Id);

            map.OwnsOne(m => m.Start, pos =>
            {
                pos.Property(p => p.X);
                pos.Property(p => p.Y);
            });

            map.OwnsOne(m => m.Goal, pos =>
            {
                pos.Property(p => p.X);
                pos.Property(p => p.Y);
            });

            map.OwnsMany(m => m.Obstacles, b =>
            {
                b.WithOwner().HasForeignKey("MapId");

                b.Property<int>("Id");
                b.HasKey("Id");

                b.Property(p => p.X);
                b.Property(p => p.Y);

                b.ToTable("MapObstacles");
            });
        }
    }
}
