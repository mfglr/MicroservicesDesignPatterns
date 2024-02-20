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
        public readonly static string Payment_StockReservedRequestEventQueueName = "Payment_StockReservedRequestEventQueueName";
        public readonly static string Order_OrderCreattionRequestCompletedQueue = "Order_OrderCreattionRequestCompletedQueue";
        public readonly static string Order_OrderCreattionRequestFailedQueue = "Order_OrderCreattionRequestFailedQueue";

        public readonly static string StockCompansableTransactionMessageQueue = "StockCompansableTransactionMessageQueue";
    }
}
