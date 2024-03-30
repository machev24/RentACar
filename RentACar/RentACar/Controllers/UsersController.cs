using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data.Entities;
using RentACar.Data.Mapping;
using RentACar.Models;

namespace RentACar.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users
                .ProjectTo<UserListingViewModel>(mapper.ConfigurationProvider)
                .ToArray();

            var adminIds = (await userManager
                .GetUsersInRoleAsync(GlobalConstants.AdminRoleName))
                .Select(r => r.Id)
                .ToHashSet();

            foreach (var user in users)
            {
                user.IsAdmin = adminIds.Contains(user.Id);
            }

            var orderedUsers = users
                .OrderByDescending(u => u.IsAdmin)
                .ThenBy(u => u.UserName);

            return this.View(orderedUsers);
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string userId)
        {
            if (userId == null)
            {
                return this.RedirectToAction("Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null || await this.userManager.IsInRoleAsync(user, GlobalConstants.AdminRoleName))
            {
                return this.RedirectToAction("Index");
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.AdminRoleName);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Demote(string userId)
        {
            if (userId == null)
            {
                return this.RedirectToAction("Index");
            }

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null || !await this.userManager.IsInRoleAsync(user, GlobalConstants.AdminRoleName))
            {
                return this.RedirectToAction("Index");
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdminRoleName);

            return this.RedirectToAction("Index");
        }
    }
}
