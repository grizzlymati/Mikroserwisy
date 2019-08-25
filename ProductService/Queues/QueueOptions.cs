namespace ProductService.Queues
{
    public class QueueOptions
    {
        public string ReleasedProductsDataEventQueueName { get; set; }

        public string TakenProductsDataEventEventQueueName { get; set; }
    }
}
