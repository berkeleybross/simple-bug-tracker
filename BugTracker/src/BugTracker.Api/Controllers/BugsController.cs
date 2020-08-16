using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Api.PgSqlDatabase;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Api.Controllers
{
    [ApiController]
    [Route("api/bugs")]
    public class BugsController
        : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public BugsController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public List<BugDto> Get()
        {
            return this.databaseContext.Bug.AsEnumerable()
                .Select(b => new BugDto
                {
                    ActiveUserId = b.ActiveUser?.Id,
                    Slug = b.Slug,
                    Title = b.Title
                })
                .ToList();
        }

        public class BugDto
        {
            public Guid? ActiveUserId { get; set; }

            public string Slug { get; set; }

            public string Title { get; set; }
        }
    }
}
