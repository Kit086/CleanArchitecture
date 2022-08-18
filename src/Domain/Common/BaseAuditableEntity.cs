using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CleanArchitecture.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    [NotMapped]
    public DateTime Created => CreatedUtc.ToLocalTime();

    [NotMapped]
    public DateTime? LastModified => LastModifiedUtc?.ToLocalTime();
}
