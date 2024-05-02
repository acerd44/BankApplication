using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.ViewModels
{
    public class CRUDCustomerViewModel
    {
        [MaxLength(20)]
        [Required]
        public string Givenname { get; set; } = null!;
        [MaxLength(20)]
        [Required]
        public string Surname { get; set; } = null!;
        [StringLength(100)]
        [Required]
        public string Streetaddress { get; set; } = null!;
        [Required]
        [Range(1, 4, ErrorMessage = "Please choose a country")]
        public Country Country { get; set; }
        [StringLength(50)]
        [Required]
        public string City { get; set; } = null!;
        [StringLength(100)]
        [EmailAddress]
        public string? Emailaddress { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Please choose a gender")]
        public Gender Gender { get; set; }
        [Required]
        public string Zipcode { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public bool IsActive { get; set; }
        public string? Telephonenumber { get; set; }
        [RegularExpression(@"^\d{6}[A-Z-]\d{3}[a-zA-Z0-9]$", ErrorMessage = "Enter a ten-digit national id, like so: XXXXXX-XXXX")]
        public string? NationalId { get; set; }
    }
}
