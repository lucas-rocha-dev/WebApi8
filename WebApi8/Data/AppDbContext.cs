using Microsoft.EntityFrameworkCore;

namespace WebApi8.Data 
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<Models.AutorModel> Autores { get; set; }
        public DbSet<Models.LivroModel> Livros { get; set; }
    }
}
