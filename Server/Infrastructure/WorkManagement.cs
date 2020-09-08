using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pws.Shared.Domain;

namespace pws.Server.Infrastructure
{
    public class WorkManagement : DbContext
    {
        public WorkManagement(DbContextOptions<WorkManagement> options): 
            base(options) { }

        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkerEntityConfiguration());
        }
    }

    public class WorkerEntityConfiguration : IEntityTypeConfiguration<Worker>
    {
        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder.HasKey(x => x.Name);
        }
    }

    public class WorkManagementContextFactory : IDesignTimeDbContextFactory<WorkManagement>
    {
        public WorkManagement CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorkManagement>();
            optionsBuilder.UseSqlite("Data Source=workmanagement.db;");

            return new WorkManagement(optionsBuilder.Options);
        }
    }
}
