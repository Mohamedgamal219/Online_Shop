using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myShop.Entity.Models;
using myShop.Uititlity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.DbIntializer
{
    public class Dbintialize : IDbIntialize
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;

        public Dbintialize(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }
        public void Intiaize()
        {
            // Migration
            try
            {
                if(_appDbContext.Database.GetPendingMigrations().Count()> 0)
                {
                    _appDbContext.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            //Role 

            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();





                _userManager.CreateAsync(new ApplicationUsers
                {
                    Name = "Adiminsterator",
                    Email = "Admin@MyShop.com",
                    UserName = "Admin@MyShop.com",
                    City = "Cairo",
                    Address = "Cairo",
                    PhoneNumber = "123456789"
                }, "Passw@rd123").GetAwaiter().GetResult();


                ApplicationUsers user = _appDbContext.ApplicationUsers.FirstOrDefault(x => x.Email == "Admin@MyShop.com");

                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

                return;
            }
        }
    }
}
