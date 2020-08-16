using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BugTracker.Api.PgSqlDatabase
{
    public class DatabaseContext
        : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }

        public DbSet<Bug> Bug { get; set; }

        public DbSet<SiteUser> SiteUser { get; set; }
    }

    [Table("site_user")]
    public class SiteUser
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    [Table("bug")]
    public class Bug
    {
        [Key]
        public Guid Id { get; set; }

        public string Slug { get; set; }

        public Instant Created { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public SiteUser ActiveUser { get; set; }
    }
}
