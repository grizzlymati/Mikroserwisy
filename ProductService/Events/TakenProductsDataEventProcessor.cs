using ProductService.Events.Interfaces;
using ProductService.Queues.Interfaces;

namespace ProductService.Events
{
    public class TakenProductsDataEventProcessor : ITakenProductsDataEventProcessor
    {
        private ITakenProductsDataEventSubscriber _subscriber;

        public TakenProductsDataEventProcessor(ITakenProductsDataEventSubscriber takenProductsDataEventSubscriber)
        {
            _subscriber = takenProductsDataEventSubscriber;
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
