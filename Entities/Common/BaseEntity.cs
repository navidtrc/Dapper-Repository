using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public interface IEntity
    {
        public abstract string SchemaName { get; }
        public abstract string TableName { get; }
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

    public abstract class BaseEntity<TKey> : IEntity where TKey : struct
    {
        [Key]
        public virtual TKey Id { get; set; }

        public virtual Guid Guid { get; set; } = Guid.NewGuid();

        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual DateTime? LastModifiedDate { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsDeleted { get; set; } = false;

        [NotMapped]
        public abstract string SchemaName { get; }
        [NotMapped]
        public abstract string TableName { get; }
    }

    public abstract class BaseEntity : BaseEntity<long>
    {
    }
}
