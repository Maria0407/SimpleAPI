using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleAPI.Models
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public PeopleContext(DbContextOptions<PeopleContext> options)
            : base(options)
        {
            Database.EnsureCreated(); //check if exists - create if doesn't
        }
    }
}
