using Application.Common.Interfaces;
using Application.Utility;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class DbInitializer : IDbinitializer
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(ApplicationDbContext applicationDbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();

                    var newAdmin = new ApplicationUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        NormalizedUserName = "admin@gmail.com".ToUpper(),
                        PhoneNumber = "123456789",
                        Name = "Admin",
                        NormalizedEmail = "admin@gmail.com".ToUpper(),
                        EmailConfirmed = true,
                    };

                    _userManager.CreateAsync(newAdmin, "Admin@1").GetAwaiter().GetResult();
                    var getAdmin = _applicationDbContext.Users.FirstOrDefault(i => i.Email == "admin@gmail.com");

                    _userManager.AddToRoleAsync(getAdmin, SD.Admin).GetAwaiter().GetResult();
                }





            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);

            }
        }
    }
}
