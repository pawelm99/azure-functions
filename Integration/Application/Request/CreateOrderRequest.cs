namespace Integration.Application.Request
{
    public class CreateOrderRequest
    {
        public List<Guid> SellersIds { get; set; }
        public List<Guid> CsutomersIds { get; set; }
        public List<Guid> InvoicesIds { get; set; }
    }
}
