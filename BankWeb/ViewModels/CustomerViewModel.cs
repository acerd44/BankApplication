namespace BankWeb.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Streetaddress { get; set; } = null!;
    }
}
