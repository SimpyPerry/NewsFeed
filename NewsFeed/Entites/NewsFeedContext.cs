using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Entites
{
    public class NewsFeedContext : IdentityDbContext<IdentityUser>
    {
        public NewsFeedContext(DbContextOptions<NewsFeedContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<NewsFeedEntity> news { get; set; }
        //public DbSet<HasTag> hasTags { get; set; }
    }
}
