using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using TestAdminCore;
using TestAdminCore.Controllers;
using TestAdminCore.Models;

namespace UnitTest
{
    public class Class1
    {
        private ApplicationUser user = new ApplicationUser();
        private ApplicationUser user1 = new ApplicationUser();
        // ask about using lsit for db, and therefore you can test even more!
        
        [Fact]
        public void TestApplicationUser()
        {
            user.FirstName = "John";
            user.LastName = "Cena";
            user1.FirstName = "Teddy";
            user1.LastName = "Beer";
            Assert.NotEqual(user, user1);
            
          
        }
    }
}
