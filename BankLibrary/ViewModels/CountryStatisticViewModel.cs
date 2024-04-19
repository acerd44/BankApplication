namespace BankLibrary.ViewModels
{
    public class CountryStatisticViewModel
    {
        public string Country { get; set; }
        public int AccountCount { get; set; }
        public int CustomerCount { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
