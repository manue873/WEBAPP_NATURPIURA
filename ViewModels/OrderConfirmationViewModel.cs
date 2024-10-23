namespace WEBAPP_NATURPIURA.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Envio { get; set; }
    }
}
