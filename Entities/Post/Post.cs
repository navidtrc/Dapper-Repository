using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public override string SchemaName => "dbo";
        public override string TableName => nameof(Post);
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();
        }
    }
}
