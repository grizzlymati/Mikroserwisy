using ProductService.Events.Interfaces;
using ProductService.Queues.Interfaces;

namespace ProductService.Events
{
    public class ReleasedProductsDataEventProcessor : IReleasedProductsDataEventProcessor
    {
        private IReleasedProductsDataEventSubscriber subscriber;

        public ReleasedProductsDataEventProcessor(IReleasedProductsDataEventSubscriber eventSubscriber)
        {
            this.subscriber = eventSubscriber;
        }

        public void Start()
        {
            this.subscriber.Subscribe();
        }

        public void Stop()
        {
            this.subscriber.Unsubscribe();
        }
    }
}
