using ProductService.Events.Interfaces;
using ProductService.Queues.Interfaces;

namespace ProductService.Events
{
    public class ReleasedProductsDataEventProcessor : IReleasedProductsDataEventProcessor
    {
        private IReleasedProductsDataEventSubscriber _subscriber;

        public ReleasedProductsDataEventProcessor(IReleasedProductsDataEventSubscriber eventSubscriber)
        {
            _subscriber = eventSubscriber;
        }

        public void Start()
        {
            _subscriber.Subscribe();
        }

        public void Stop()
        {
            _subscriber.Unsubscribe();
        }
    }
}
