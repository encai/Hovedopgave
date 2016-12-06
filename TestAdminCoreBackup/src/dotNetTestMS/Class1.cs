using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TestAdminCore.Controllers;
using TestAdminCore.Models;

namespace dotNetTestMS
{
    [TestClass]
    public class TestClass
    {
        private ApplicationUser ac;
        private ApplicationUser ac2;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        [TestInitialize]
        public void BeforeTest()
        {
            ac = new ApplicationUser()
            {
                FirstName = "John",
                LastName = "Cena",
                UserName = "Test123@test.com",
                City = "Roskilde"
               
                
            };
             _userManager.CreateAsync(ac, "!Test123");
            _roleManager.CreateAsync(new IdentityRole("Administrator"));
            ac2 = new ApplicationUser()
            {
                FirstName = "Test2",
                LastName = "Cena",
                UserName = "Test1425@test.com",
                City = "Copenhagen",
            };
             _userManager.CreateAsync(ac2, "!Test123");
             _roleManager.CreateAsync(new IdentityRole("Administrator"));
        }


        [TestMethod]
        public void testUserNotIdentitcal()
        {

            Assert.AreNotEqual(ac, ac2);
        }

        [TestMethod]
        public void testUserRoles()
        {
            ac.Roles.Contains(_roleManager.GetRoleNameAsync(""));
        }



        //[TestMethod]
        //public void TestMethodPassing()
        //{
        //    Assert.IsTrue(true);
        //}

        //[TestMethod]
        //public void TestMethodFailing()
        //{
        //    Assert.IsTrue(false);
        //}

    }
}