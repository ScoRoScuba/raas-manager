namespace OFX.RAASManager.Core.Mongo.Interfaces
{
    public interface IAuditSummaryRepository
    {
        T Match<T>(ICriteria<T> criteria);
    }
}