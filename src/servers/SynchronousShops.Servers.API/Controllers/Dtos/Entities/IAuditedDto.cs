using System;

namespace SynchronousShops.Servers.API.Controllers.Dtos.Entities
{
    public interface ICreationAuditedDto
    {
        DateTimeOffset CreatedAt { get; set; }
        string CreatedBy { get; set; }
    }

    public interface IModificationAuditedDto
    {
        DateTimeOffset? ModifiedAt { get; set; }
        string ModifiedBy { get; set; }
    }

    public interface IAuditedDto : ICreationAuditedDto, IModificationAuditedDto
    {
    }

    public interface IDeletionAuditedDto
    {
        DateTimeOffset? DeletedAt { get; set; }
        string DeletedBy { get; set; }
    }
}
