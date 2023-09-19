using Comments.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comments.Persistance.EFConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("comments");

            builder.Property(p => p.ContextIdentifier)
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(p => p.UserIdentifier)
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(p => p.UserDisplayName)
                 .HasMaxLength(1024);

            builder.Property(p => p.CommentTime)
                .IsRequired();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId);

            builder.Property(x => x.CommentText)
                .IsRequired();

            builder.Property(x => x.Deleted)
                .HasDefaultValue(false);

        }
    }
}
