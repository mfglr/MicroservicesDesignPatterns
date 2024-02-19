namespace SharedLibrary
{
    public class QueueNames
    {

        public readonly static string OrderSaga = "order-saga-queue";


        public readonly static string Stock_OrderCreateEventQueueName = "Stock_OrderCreateEventQueueName";
        public readonly static string StockReservedEventQueueName = "StockReservedEventQueueName";
        public readonly static string Order_PaymentCompletedEventQueueName = "Order_PaymentCompletedEventQueueName";
        public readonly static string Order_PaymentFailedEventQueueName = "Order_PaymentFailedEventQueueName";
        public readonly static string Order_StockNotReservedEventQueueName = "Order_StockNotReservedEventQueueName";
        public readonly static string Stock_PaymentFailedEventQueueName = "Stock_PaymentFailedEventQueueName";
    }
}
