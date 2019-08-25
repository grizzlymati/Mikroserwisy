using ProductService.Events.Interfaces;
using ProductService.Queues.Interfaces;

namespace ProductService.Events
{
    public class TakenProductsDataEventProcessor : ITakenProductsDataEventProcessor
    {
        private ITakenProductsDataEventSubscriber subscriber;

        public TakenProductsDataEventProcessor(ITakenProductsDataEventSubscriber takenProductsDataEventSubscriber)
        {
            this.subscriber = takenProductsDataEventSubscriber;
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
