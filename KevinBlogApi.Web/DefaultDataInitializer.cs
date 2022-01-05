using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KevinBlogApi.Web
{
    public static class DefaultDataInitializer
    {
        public static void SeedData (UserManager<IdentityUser> userManager
            , RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                {
                    IdentityRole role = new IdentityRole("Admin");
                    var ret = roleManager.CreateAsync(role).Result;
                    Debug.Assert(ret.Succeeded);
                }

                {
                    IdentityRole role = new IdentityRole("User");
                    var ret = roleManager.CreateAsync(role).Result;
                    Debug.Assert(ret.Succeeded);
                }
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("kevin").Result == null)
            {
                IdentityUser user = new IdentityUser("kevin");
                user.Email = "kevin@kevinlan.cn";

                var ret = userManager.CreateAsync(user, "Abc12345678.").Result;
                Debug.Assert(ret.Succeeded);

                if (ret.Succeeded)
                {
                    ret = userManager.AddToRoleAsync(user, "admin").Result;
                    Debug.Assert(ret.Succeeded);
                }
            }
        }
    }
}
