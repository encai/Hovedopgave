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
using Microsoft.AspNet.Mvc;
using System.Linq;

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

        [TestMethod]
        public void Get_non_existent_person_returns_null()
        {
            // Act
            var result = _controller.GetPerson("Fred") as ViewResult;

            // Assert
            Assert.IsNull(result.ViewData.Model);
        }

        [TestMethod]
        public void Add_person_saves_to_db_with_generated_id()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            Person person = new Person()
            {
                Id = personId,
                FirstName = "Billy",
                LastName = "McBill"
            };
            var beforePersonCount = _context.People.Count();
            // Act
            var result = _controller.AddPerson(person) as HttpStatusCodeResult;

            // Assert
           // Assert.AreEqual(200, result.StatusCode);
            Person savedPerson = _context.People.Single(x => x.FirstName == "Billy" && x.LastName == "McBill");

            Assert.AreNotEqual(personId, savedPerson.Id);
            Assert.AreEqual(beforePersonCount +1 , _context.People.Count());
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