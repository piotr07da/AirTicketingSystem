namespace Ats.Core.Domain
{
    public interface IAggregateEventApplierFactory
    {
        IAggregateEventApplier Create();
    }
}
