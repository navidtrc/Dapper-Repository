using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<Post> Posts { get; set; }

        public override string SchemaName => "dbo";
        public override string TableName => nameof(Category);
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();
        }
    }
}
