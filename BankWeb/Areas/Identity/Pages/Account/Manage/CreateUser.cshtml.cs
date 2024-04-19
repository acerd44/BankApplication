using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankWeb.Areas.Identity.Pages.Account.Manage
{
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public CreateUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }
        [BindProperty]
        public IdentityUserViewModel User { get; set; }
        public List<string> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ModelState.Remove("User.UserId");
            ModelState.Remove("User.UserName");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser { UserName = User.Email, Email = User.Email };
            user.PasswordHash = _passwordHasher.HashPassword(user, User.Password);
            await _userManager.CreateAsync(user);
            await _userManager.AddToRolesAsync(user, User.Roles);
            return RedirectToPage("./ManageUsers");
        }
    }
}
