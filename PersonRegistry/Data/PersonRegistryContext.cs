using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonRegistry.Models;

namespace PersonRegistry.Data
{
    public class PersonRegistryContext : DbContext
    {
        public PersonRegistryContext (DbContextOptions<PersonRegistryContext> options)
            : base(options)
        {
        }

        public DbSet<PersonRegistry.Models.User> User { get; set; }
    }
}
