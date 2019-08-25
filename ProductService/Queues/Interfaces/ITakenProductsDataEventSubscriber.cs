namespace ProductService.Queues.Interfaces
{
    public interface ITakenProductsDataEventSubscriber
    {
        void Subscribe();
        void Unsubscribe();
    }
}
