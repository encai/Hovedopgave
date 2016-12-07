using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAdminCore.Data.Migrations
{
    public class MyDBContext : DbContext

    {
        public MyDBContext(DbContextOptions options) : base(options)
        {

        }
        

        public DbSet<Person> People { get; set; }

        public class Person
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }
    }
}
