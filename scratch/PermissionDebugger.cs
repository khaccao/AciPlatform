using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;

namespace AciPlatform.Scratch
{
    public class PermissionDebugger
    {
        public static async Task DebugUserPermissions(IServiceProvider services, string username)
        {
            var context = services.GetRequiredService<IApplicationDbContext>();
            
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                Console.WriteLine($"User '{username}' not found.");
                return;
            }

            Console.WriteLine($"--- Debugging User: {user.Username} (ID: {user.Id}) ---");
            Console.WriteLine($"FullName: {user.FullName}");
            Console.WriteLine($"UserRoleIds: {user.UserRoleIds}");

            var roleIds = user.UserRoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse).ToList() ?? new List<int>();

            var roles = await context.UserRoles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
            Console.WriteLine("Roles Found:");
            foreach (var r in roles)
            {
                Console.WriteLine($" - ID: {r.Id}, Code: {r.Code}, Title: {r.Title}");
            }

            bool isSuperAdmin = roles.Any(r => r.Code == "SuperAdmin");
            Console.WriteLine($"Is SuperAdmin (detected by Code): {isSuperAdmin}");

            var menuRoles = await context.MenuRoles
                .Where(mr => mr.UserRoleId != null && roleIds.Contains(mr.UserRoleId.Value))
                .ToListAsync();

            Console.WriteLine($"Total MenuRole entries for these roles: {menuRoles.Count}");
            
            var viewableMenus = menuRoles.Where(mr => mr.View == true).Select(mr => mr.MenuCode ?? mr.MenuId.ToString()).Distinct().ToList();
            Console.WriteLine($"Menus with View=true: {string.Join(", ", viewableMenus)}");

            var userMenus = await context.UserMenus.Where(um => um.UserId == user.Id).ToListAsync();
            Console.WriteLine($"Total UserMenu (direct) entries: {userMenus.Count}");
            var directViewable = userMenus.Where(um => um.View == true).Select(um => um.MenuCode ?? um.MenuId.ToString()).Distinct().ToList();
            Console.WriteLine($"Direct viewable menus: {string.Join(", ", directViewable)}");
        }
    }
}
