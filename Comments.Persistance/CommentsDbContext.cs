using Comments.Persistance.Common;
using Comments.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.Persistance
{
    public class CommentsDbContext : DbContext
    {
        public virtual DbSet<CommentEntity> Comments { get; set; }

        public CommentsDbContext(DbContextOptions<CommentsDbContext> options)
            : base(options) { }
        public CommentsDbContext() : base() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseHelper.GetConnectionStringFromEnv());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentsDbContext).Assembly);            
        }
    }
}