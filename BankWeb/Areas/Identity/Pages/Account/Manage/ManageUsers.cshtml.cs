using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BankLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace BankWeb.Areas.Identity.Pages.Account.Manage
{
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManageUsersModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<IdentityUserViewModel> UsersWithRoles { get; set; }

        public async Task OnGetAsync()
        {
            UsersWithRoles = new();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersWithRoles.Add(new IdentityUserViewModel { UserId = user.Id, UserName = user.UserName, Roles = roles });
            }
        }
    }
}
