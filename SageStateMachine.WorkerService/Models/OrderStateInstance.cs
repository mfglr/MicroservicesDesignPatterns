using MassTransit;
using System.Text;

namespace SageStateMachine.WorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int BuyerId { get; set; }
        public int OrderId { get; set; }
        public string CardName { get; set; }
        public string CartNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var properties = GetType().GetProperties();

            foreach( var property in properties)
            {
                var value = property.GetValue(this, null);
                sb.Append($"{property.Name}:{value}\n");
            }
            return sb.ToString();
             
        }
    }
}
