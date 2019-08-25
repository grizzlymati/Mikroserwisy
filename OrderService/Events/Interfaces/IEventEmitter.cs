namespace OrderService.Events.Interfaces
{
    public interface IEventEmitter
    {
        void EmitTakenProductsDataEvent(TakenProductsDataEvent takenProductsDataEvent);
        void EmitReleasedProductsDataEvent(ReleasedProductsDataEvent releasedProductsDataEvent);
    }
}
