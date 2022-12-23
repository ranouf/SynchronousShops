namespace SynchronousShops.Libraries.Entities
{
    public abstract class FullAuditedEntityWithSoftDelete : FullAuditedEntity, ISoftDelete
    {
        ISoftDelete ISoftDelete.Update(bool isDeleted)
        {
            IsDeleted = isDeleted;
            return this;
        }
    }
}
