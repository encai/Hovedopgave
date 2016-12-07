using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TestAdminCore.Controllers;
using TestAdminCore.Data.Migrations;
using TestAdminCore.Models;
using static TestAdminCore.Data.Migrations.MyDBContext;

namespace dotNetTestMS
{
    [TestClass]
    public class TestClass
    {

        private MyDBContext _context;
        private HomeController _controller;

        public TestClass()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();
            _context = new MyDBContext(optionsBuilder.Options);

            // Seed data
            _context.People.Add(new Person()
            {
                FirstName = "John",
                LastName = "Doe"
            });
            _context.People.Add(new Person()
            {
                FirstName = "Sally",
                LastName = "Doe"
            });
            _context.SaveChanges();

            // Create test subject
            _controller = new HomeController(_context);
        }
        [TestMethod]
        public void Get_person_john_returns_john()
        {

            // Act
            var result = _controller.GetPerson("John") as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Person));
            Person model = (Person)result.ViewData.Model;
            Assert.AreEqual("John", model.FirstName);
            Assert.AreEqual("Doe", model.LastName);
        }

        #region Standard Test
        //private ApplicationUser ac;
        //private ApplicationUser ac2;
        //private RoleManager<IdentityRole> _roleManager;
        //private UserManager<ApplicationUser> _userManager;

        //[TestInitialize]
        //public void BeforeTest()
        //{
        //    ac = new ApplicationUser()
        //    {
        //        FirstName = "John",
        //        LastName = "Cena",
        //        UserName = "Test123@test.com",
        //        City = "Roskilde"


        //    };
        //     _userManager.CreateAsync(ac, "!Test123");
        //    _roleManager.CreateAsync(new IdentityRole("Administrator"));
        //    ac2 = new ApplicationUser()
        //    {
        //        FirstName = "Test2",
        //        LastName = "Cena",
        //        UserName = "Test1425@test.com",
        //        City = "Copenhagen",
        //    };
        //     //_userManager.CreateAsync(ac2, "!Test123");
        //     //_roleManager.CreateAsync(new IdentityRole("Administrator"));
        //}


        //[TestMethod]
        //public void testUserNotIdentitcal()
        //{

        //    Assert.AreNotEqual(ac, ac2);
        //}

        //[TestMethod]
        //public void testUserRoles()
        //{
        //    ac.Roles.Contains(_roleManager.GetRoleNameAsync(""));
        //}



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
        #endregion
    }
}