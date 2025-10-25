using Microsoft.EntityFrameworkCore;

namespace isgasoir
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          /*  modelBuilder.Entity<Semestre>()

                .HasMany<Module>(s=>s.Modules)
                .WithOne(m=>m.Sem)
                .HasForeignKey(e=>e.Id);
           */
           


                
                
                
                


        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Studant> Studants { get; set; }
        public DbSet<Semestre> Semestrees { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Chapitre> Chapitres { get; set; }
        public DbSet<Activity> Activities { get; set; }

    }
}
