namespace BankWeb.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string? Country { get; set; }
    }
}
