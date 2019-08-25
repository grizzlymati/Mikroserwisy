namespace ProductService.Queues.Interfaces
{
    public interface IReleasedProductsDataEventSubscriber
    {
        void Subscribe();
        void Unsubscribe();
    }
}
