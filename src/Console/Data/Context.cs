using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Console.Migrations;
using Console.Models;

namespace Console.Data
{
    public class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        public virtual DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .Property(e=>e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }
    }

}