using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TIPMC_WebApp_Vesperr.Models;
using TIPMC_WebApp_Vesperr.Common;
using TIPMC_WebApp_Vesperr.Models.Online;

namespace TIPMC_WebApp_Vesperr.Data
{
    public static class DataSeed
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                context.Database.Migrate();

                if (!context.Products.Any())
                {
                    Category fruits = new Category { Name = "Fruits", Slug = "fruits" };
                    Category shirts = new Category { Name = "Shirts", Slug = "shirts" };

                    context.Products.AddRange(
                            new ProductOnline
                            {
                                Name = "Apples",
                                Slug = "apples",
                                Description = "Juicy apples",
                                Price = 1.50M,
                                Category = fruits,
                                Image = "apples.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Bananas",
                                Slug = "bananas",
                                Description = "Fresh bananas",
                                Price = 3M,
                                Category = fruits,
                                Image = "bananas.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Watermelon",
                                Slug = "watermelon",
                                Description = "Juicy watermelon",
                                Price = 0.50M,
                                Category = fruits,
                                Image = "watermelon.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Grapefruit",
                                Slug = "grapefruit",
                                Description = "Juicy grapefruit",
                                Price = 2M,
                                Category = fruits,
                                Image = "grapefruit.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "White shirt",
                                Slug = "white-shirt",
                                Description = "White shirt",
                                Price = 5.99M,
                                Category = shirts,
                                Image = "white shirt.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Black shirt",
                                Slug = "black-shirt",
                                Description = "Black shirt",
                                Price = 7.99M,
                                Category = shirts,
                                Image = "black shirt.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Yellow shirt",
                                Slug = "yellow-shirt",
                                Description = "Yellow shirt",
                                Price = 11.99M,
                                Category = shirts,
                                Image = "yellow shirt.jpg"
                            },
                            new ProductOnline
                            {
                                Name = "Grey shirt",
                                Slug = "grey-shirt",
                                Description = "Grey shirt",
                                Price = 12.99M,
                                Category = shirts,
                                Image = "grey shirt.jpg"
                            }
                    );

                    context.SaveChanges();
                }

                UserManager<ApplicationUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Seed database code goes here

                // User Info
                //string userName = "SuperAdmin";
                string firstName = "Super";
                string lastName = "Admin";
                string fullname = firstName + " " + lastName;
                string email = "superadmin@admin.com";
                string password = "Super_1234";
                string role = "SuperAdmins";
                string role2 = "SeniorManagers";
                string role3 = "Managers";
                string role4 = "Members";
                string role5 = "Non-Members";
                string role6 = "BOD";
                string role7 = "Administrator";
                string role8 = "Moderator";

                if (await _userManager.FindByNameAsync(email) == null)
                {
                    // Create SuperAdmins role if it doesn't exist
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                    if (await roleManager.FindByNameAsync(role2) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role2));
                    }
                    if (await roleManager.FindByNameAsync(role3) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role3));
                    }
                    if (await roleManager.FindByNameAsync(role4) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role4));
                    }
                    if (await roleManager.FindByNameAsync(role5) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role5));
                    }
                    if (await roleManager.FindByNameAsync(role6) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role7));
                    }
                    if (await roleManager.FindByNameAsync(role4) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role7));
                    }
                    if (await roleManager.FindByNameAsync(role8) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role8));
                    }

                    // Create user account if it doesn't exist
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        //extended properties
                        FirstName = firstName,
                        LastName = lastName,
                        FullName = firstName + " " + lastName,
                        AvatarURL = "/images/user.png",
                        DateRegistered = DateTime.UtcNow.ToString(),
                        Position = "",
                        NickName = "",
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    // Assign role to the user
                    if (result.Succeeded)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.GivenName, user.FirstName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Surname, user.LastName));
                        await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.AvatarURL, user.AvatarURL));

                        //SignInManager<ApplicationUser> _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
                        //await _signInManager.SignInAsync(user, isPersistent: false);

                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
