using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankWeb.Areas.Identity.Pages.Account.Manage
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public EditUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }
        [BindProperty]
        public IdentityUserViewModel User { get; set; }
        public List<string> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            User = new IdentityUserViewModel
            {
                UserId = userId,
                Email = user.Email,
                Password = user.PasswordHash,
                UserName = user.UserName,
                Roles = roles.ToList()
            };
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            ModelState.Remove("User.UserId");
            ModelState.Remove("User.UserName");
            ModelState.Remove("User.Password");
            if (Request.Form.TryGetValue("deleteUser", out var _))
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    foreach (var error in deleteResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
                return RedirectToPage("./ManageUsers");

            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            user.Email = User.Email;
            user.UserName = User.Email;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var rolesToRemove = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, User.Roles);
            return RedirectToPage("./ManageUsers");
        }
    }
}
