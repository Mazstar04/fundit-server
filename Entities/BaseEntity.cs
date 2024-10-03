

using fundit_server.Interfaces.Data;

namespace fundit_server.Entities;

public abstract class BaseEntity : ISoftDeletable, IAuditBase
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

}