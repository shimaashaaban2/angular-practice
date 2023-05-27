using Microsoft.EntityFrameworkCore;

namespace movie_crud.Models
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<ApplicationDbContext>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        { 
        }   
        
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genere> Generes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Movies;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);

        }
    }
   
}
