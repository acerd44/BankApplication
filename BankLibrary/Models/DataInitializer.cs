using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankLibrary.Models;
public class DataInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DataInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _context = dbContext;
        _userManager = userManager;
    }
    public void SeedData()
    {
        _context.Database.Migrate();
        SeedRoles();
        SeedUsers();
        SeedTransactions();
    }

    private void SeedUsers()
    {
        AddUserIfNotExists("richard.chalk@systementor.se", "Hejsan123#", new string[] { "Admin" });
        AddUserIfNotExists("admin.test@gmail.com", "TestAccount123!", new string[] { "Admin" });
        AddUserIfNotExists("richard.erdos.chalk@gmail.se", "Hejsan123#", new string[] { "Cashier" });
        AddUserIfNotExists("cashier.test@gmail.com", "TestAccount123!", new string[] { "Cashier" });
    }

    private void SeedRoles()
    {
        AddRoleIfNotExisting("Admin");
        AddRoleIfNotExisting("Cashier");
    }

    private void SeedTransactions()
    {
        AddTransaction(16000, 15, 0);
        AddTransaction(6000, 25, -2);
        AddTransaction(8000, 25, -2);
        AddTransaction(12000, 25, -2);
    }

    private void AddRoleIfNotExisting(string roleName)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role == null)
        {
            _context.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
            _context.SaveChanges();
        }
    }

    private void AddUserIfNotExists(string userName, string password, string[] roles)
    {
        if (_userManager.FindByEmailAsync(userName).Result != null) return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(user, password).Wait();
        _userManager.AddToRolesAsync(user, roles).Wait();
    }

    private void AddTransaction(decimal amount, int accountId, int days)
    {
        if (!_context.Transactions.Any(t => t.Amount == amount
        && t.AccountId == accountId
        && t.Date == DateOnly.FromDateTime(DateTime.Now.AddDays(days))))
        {
            {
                _context.Transactions.Add(new Transaction
                {
                    AccountId = accountId,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(days)),
                    Amount = amount,
                    Type = "Credit",
                    Operation = "Credit in Cash",
                    Balance = amount,
                    Symbol = "lol"
                });
            }
        }
        _context.SaveChanges();
    }
}