﻿namespace BankLibrary.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateOnly Created { get; set; }
        public string? Country { get; set; }
        public bool IsActive { get; set; }
    }
}
