using BankLibrary.Infrastructure;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Areas.Identity.Pages.Account.Manage
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [BindProperty]
        public IdentityUserViewModel EditUser { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [BindProperty]
        [ValidateNever]
        public string NewPassword { get; set; }
        [BindProperty]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        [ValidateNever]
        public string OldPassword { get; set; }
        public List<string> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            EditUser = new IdentityUserViewModel
            {
                UserId = userId,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            ModelState.Remove("EditUser.UserId");
            ModelState.Remove("EditUser.UserName");
            ModelState.Remove("EditUser.Password");

            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

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
                TempData["Message"] = "User was successfully deleted.";
                return RedirectToPage("./ManageUsers");
            }
            var dontChangePassword = (string.IsNullOrEmpty(OldPassword) && string.IsNullOrEmpty(NewPassword)) || (OldPassword.Equals("ligmaballs") && NewPassword.Equals("ligmaballs"));
            if (dontChangePassword)
            {
                ModelState.Remove("OldPassword");
                ModelState.Remove("NewPassword");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.Email = EditUser.Email;
            user.UserName = EditUser.Email;
            IdentityResult? changePasswordResult = new();
            if (!dontChangePassword) changePasswordResult = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            var result = await _userManager.UpdateAsync(user);
            if (!changePasswordResult.Succeeded && !dontChangePassword)
            {
                ModelState.AddModelError("All", "Old password is incorrect");
                return Page();
            }
            else if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var rolesToRemove = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, EditUser.Roles);
            TempData["Message"] = "User was successfully updated.";
            return RedirectToPage("./ManageUsers");
        }
    }
}
