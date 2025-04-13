using Microsoft.EntityFrameworkCore;

namespace BookieStore.Models
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Fiction",
                    Description = "Fictional books"
                },
                new Category
                {
                    Id = 2,
                    Name = "Non-Fiction",
                    Description = "Non-Fictional books"
                },
                new Category
                {
                    Id = 3,
                    Name = "Science",
                    Description = "Science books"
                }
                );
        }
    }
}
