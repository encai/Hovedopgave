﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAdminCore.Models;

namespace TestAdminCore.Data
{
    public class AdministratorSeedData
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public AdministratorSeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (await _userManager.FindByEmailAsync("admin@test.dk") == null)
            {
                ApplicationUser administrator = new ApplicationUser()
                {
                    UserName = "admin@test.dk",
                    Email = "admin@test.dk"
                };
                


                await _userManager.CreateAsync(administrator, "Passw0rd123!");
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));

                IdentityResult result = await _userManager.AddToRoleAsync(administrator, "Administrator");
            }

            if (await _userManager.FindByEmailAsync("Employee@test.dk") == null)
            {
                ApplicationUser employee = new ApplicationUser()
                {
                    UserName = "Employee@test.dk",
                    Email = "Employee@test.dk"
                };



                await _userManager.CreateAsync(employee, "Passw0rd123!");
                await _roleManager.CreateAsync(new IdentityRole("Employee"));

                IdentityResult result = await _userManager.AddToRoleAsync(employee, "Employee");
            }
        }
    }
}
