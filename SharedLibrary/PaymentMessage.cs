﻿namespace SharedLibrary
{
    public class PaymentMessage
    {
        public string CardName { get; set; }
        public string CartNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
